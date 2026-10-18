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

        // Add owner link
        [BindProperty]
        public OwnerLink AddLink { get; set; } = new OwnerLink();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // True only when a search has been submitted.
        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        [BindProperty]
        public List<string> SelectedLinks { get; set; }
            = new List<string>();

        public string ValidationMessage { get; set; } = string.Empty;

        public string DeleteMessage { get; set; } = string.Empty;

        public string DeleteMessageType { get; set; } = "warning";

        public string DeleteMessageTitle { get; set; } = "Please check";

        // Search dropdown: displays branch codes.
        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        // Add dropdown: displays branch names.
        public List<SelectListItem> BranchNameList { get; set; }
            = new List<SelectListItem>();

        // Role dropdown.
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

            // Preserve the existing edit-prefill behavior.
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
            try
            {
                LoadDropdowns();

                var subjectBLL = new SubjectBLL(_configuration);

                // Resolve the owner name using the existing lookups.
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
                AddLink.MakeBy = User.Identity?.Name;
                AddLink.MakeDate = DateTime.Now;
                AddLink.RecordStatus = "C";

                bool success = subjectBLL.SaveOwnerLink(AddLink);

                if (!success)
                {
                    ValidationMessage =
                        "The owner link could not be saved. Please check the information and try again.";

                    LoadLinkList();
                    return Page();
                }

                TempData["SuccessMessage"] =
                    "The owner link was saved successfully.";

                return RedirectToPage("/Subject/OwnerLinks");
            }
            catch (Exception)
            {
                // Do not expose technical exception details to the user.
                TempData["ErrorMessage"] =
                    "An unexpected error occurred while saving the owner link. Please try again.";

                return RedirectToPage("/Subject/OwnerLinks");
            }
        }

        public IActionResult OnPostDelete()
        {
            try
            {
                LoadDropdowns();

                if (SelectedLinks == null || !SelectedLinks.Any())
                {
                    DeleteMessage =
                        "Please select at least one record to delete.";

                    DeleteMessageType = "warning";
                    DeleteMessageTitle = "No record selected";

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
                            MakeBy = User.Identity?.Name,
                            MakeDate = DateTime.Now
                        });
                    }
                }

                if (!toDelete.Any())
                {
                    DeleteMessage =
                        "No valid records were selected. Please select the records again.";

                    DeleteMessageType = "warning";
                    DeleteMessageTitle = "Invalid selection";

                    LoadLinkList();
                    return Page();
                }

                var subjectBLL = new SubjectBLL(_configuration);

                bool success =
                    subjectBLL.FlagDeleteOwnerLinks(toDelete);

                if (!success)
                {
                    DeleteMessage =
                        "The selected owner link records could not be deleted. Please try again.";

                    DeleteMessageType = "error";
                    DeleteMessageTitle = "Delete failed";

                    LoadLinkList();
                    return Page();
                }

                TempData["SuccessMessage"] =
                    "The selected owner link record(s) were deleted successfully.";

                LoadLinkList();
                return Page();
            }
            catch (Exception)
            {
                // Show a friendly message rather than technical details.
                DeleteMessage =
                    "An unexpected error occurred while deleting the selected records. Please try again.";

                DeleteMessageType = "error";
                DeleteMessageTitle = "Delete failed";

                LoadDropdowns();
                LoadLinkList();

                return Page();
            }
        }

        private string ValidateOwnerLink(OwnerLink link)
        {
            var validator = new OwnerLinkValidator();
            var result = validator.Validate(link);

            if (!result.IsValid)
            {
                return string.Join(
                    Environment.NewLine,
                    result.Errors
                        .Select(e => e.ErrorMessage)
                        .Distinct());
            }

            return string.Empty;
        }

        private void LoadDropdowns()
        {
            // Keep the existing branch source.
            var branches = _miscBLL.GetBranches().Result;

            // Search dropdown displays CIB branch codes.
            BranchCodeList = branches
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.CibBranchCode
                })
                .ToList();

            // Add dropdown displays branch names but submits branch codes.
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
            {
                TotalPages = 1;
            }

            if (PageIndex < 1)
            {
                PageIndex = 1;
            }

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            LinkList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
