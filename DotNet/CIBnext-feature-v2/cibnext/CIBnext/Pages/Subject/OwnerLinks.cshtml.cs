using CIBnext.BLL;
using CIBnext.DAO;
using CIBnext.DAL;
using CIBnext.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Subject
{
    [Authorize(Roles = "2003,ADMIN")]
    public class OwnerLinksModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly MiscBLL _miscBLL;
        private readonly IConfiguration _configuration;

        private const int PageSize = 10;

        public OwnerLinksModel(
            ListOfValuesBLL lovBLL,
            MiscBLL miscBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _miscBLL = miscBLL;
            _configuration = configuration;
        }

        // Search filter
        [BindProperty(SupportsGet = true)]
        public OwnerLink Subject { get; set; } = new OwnerLink();

        // Add
        [BindProperty]
        public OwnerLink AddLink { get; set; } = new OwnerLink();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedLinks { get; set; }
            = new List<string>();

        public string ValidationMessage { get; set; }
        public string DeleteMessage { get; set; }

        // Search dropdown: displays branch codes
        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        // Add Owner Link dropdown: displays branch names
        public List<SelectListItem> BranchNameList { get; set; }
            = new List<SelectListItem>();

        // Role dropdown
        public List<SelectListItem> RoleList { get; set; }
            = new List<SelectListItem>();

        public List<OwnerLink> LinkList { get; set; }
            = new List<OwnerLink>();

        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public void OnGet(
            string instSubjectCode,
            string ownerSubjectCode,
            string branchCode,
            string role)
        {
            LoadDropdowns();

            // Edit
            if (!string.IsNullOrEmpty(instSubjectCode) &&
                !string.IsNullOrEmpty(ownerSubjectCode))
            {
                AddLink.InstitutionFiSubjectCode = instSubjectCode;
                AddLink.OwnerFiSubjectCode = ownerSubjectCode;
                AddLink.CibBranchCode = branchCode;
                AddLink.Role = role;
            }

            LoadLinkList();
        }

        public IActionResult OnPostSave()
        {
            LoadDropdowns();

            var subjectBLL = new SubjectBLL(_configuration);

            // Owner
            var ownerPerson =
                subjectBLL.GetPerson(AddLink.OwnerFiSubjectCode);

            var ownerInst =
                subjectBLL.GetInstitution(AddLink.OwnerFiSubjectCode);

            AddLink.OwnerFiSubjectName =
                ownerPerson?.Name ??
                ownerInst?.TradeName ??
                "Not found";

            var inst =
                subjectBLL.GetInstitution(
                    AddLink.InstitutionFiSubjectCode);

            AddLink.InstitutionFiSubjectName =
                inst?.TradeName ?? "Not found";

            ValidationMessage = ValidateOwnerLink(AddLink);

            if (!string.IsNullOrEmpty(ValidationMessage))
            {
                LoadLinkList();
                return Page();
            }

            AddLink.RecordType = "L";
            AddLink.FiCode = "046";
            AddLink.MakeBy = User.Identity.Name;
            AddLink.MakeDate = DateTime.Now;
            AddLink.RecordStatus = "C";

            bool success = subjectBLL.SaveOwnerLink(AddLink);

            if (!success)
            {
                ValidationMessage =
                    "Save failed. Please check the values and try again.";

                LoadLinkList();
                return Page();
            }

            return RedirectToPage("/Subject/OwnerLinks");
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedLinks == null || !SelectedLinks.Any())
            {
                DeleteMessage =
                    "Please select at least one record to delete.";

                LoadLinkList();
                return Page();
            }

            var toDelete = new List<OwnerLink>();

            foreach (var item in SelectedLinks)
            {
                var parts = item.Split('|');

                if (parts.Length == 2)
                {
                    toDelete.Add(new OwnerLink
                    {
                        InstitutionFiSubjectCode = parts[0],
                        OwnerFiSubjectCode = parts[1],
                        MakeBy = User.Identity.Name,
                        MakeDate = DateTime.Now
                    });
                }
            }

            var subjectBLL = new SubjectBLL(_configuration);

            bool success =
                subjectBLL.FlagDeleteOwnerLinks(toDelete);

            if (!success)
            {
                DeleteMessage = "Delete failed. Please try again.";
            }

            LoadLinkList();
            return Page();
        }

        private string ValidateOwnerLink(OwnerLink link)
        {
            var lv = new OwnerLinkValidator();
            var res = lv.Validate(link);

            if (!res.IsValid)
            {
                return "- " + string.Join(
                    "<br/>- ",
                    res.Errors
                        .Select(e => e.ErrorMessage)
                        .Distinct());
            }

            return string.Empty;
        }

        private void LoadDropdowns()
        {
            // Load branches through MiscBLL.
            var branches = _miscBLL.GetBranches().Result;

            // Search dropdown: show CIB branch codes.
            BranchCodeList = branches
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.CibBranchCode
                })
                .ToList();

            // Add dropdown: show branch names, but submit CIB branch codes.
            BranchNameList = branches
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.BranchName
                })
                .ToList();

            // Keep the existing role dropdown source.
            RoleList = _lovBLL.GetListOfValues(LovTypes.OWNER_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();
        }

        private void LoadLinkList()
        {
            var subjectBLL = new SubjectBLL(_configuration);

            var allResults = subjectBLL.GetOwnerLinksFiltered(
                Subject.CibBranchCode,
                Subject.OwnerFiSubjectCode,
                Subject.OwnerName,
                Subject.InstitutionFiSubjectCode,
                Subject.InstitutionName);

            TotalCount = allResults.Count;

            TotalPages = (int)Math.Ceiling(
                TotalCount / (double)PageSize);

            if (TotalPages < 1)
                TotalPages = 1;

            if (PageIndex < 1)
                PageIndex = 1;

            if (PageIndex > TotalPages)
                PageIndex = TotalPages;

            LinkList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}