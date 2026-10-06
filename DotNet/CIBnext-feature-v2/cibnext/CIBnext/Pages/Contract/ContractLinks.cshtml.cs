using CIBnext.BLL;
using CIBnext.DAO;
using CIBnext.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Contract
{
    [Authorize(Roles = "3004,ADMIN")]
    public class ContractLinksModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;
        private const int PageSize = 10;

        public ContractLinksModel(ListOfValuesBLL lovBLL, IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        
        [BindProperty(SupportsGet = true)]
        public string SearchBranchCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPrimaryCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchPrimaryName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchSecondaryCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchSecondaryName { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchContractCode { get; set; }

        
        [BindProperty(SupportsGet = true)]
        public string EditContract { get; set; }
        [BindProperty(SupportsGet = true)]
        public string EditSecondary { get; set; }
        public bool IsEdit { get; set; }

       
        [BindProperty]
        public ContractLink AddLink { get; set; } = new ContractLink();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedLinks { get; set; } = new List<string>();

        public string ValidationMessage { get; set; }
        public string DeleteMessage { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> LinkTypeList { get; set; }
        public List<ContractLink> LinkList { get; set; } = new List<ContractLink>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
            LoadLinkList();
            if (!string.IsNullOrEmpty(EditContract))
            {
                var contractBLL = new ContractBLL(_configuration);
                var all = contractBLL.GetContractLinksFiltered(null, null, null, null, null, null);

                var existing = all.FirstOrDefault(l =>
                    l.FIContractCode == EditContract &&
                    (l.FISecondaryCode ?? string.Empty) == (EditSecondary ?? string.Empty));

                if (existing != null)
                {
                    AddLink = existing;
                    IsEdit = true;
                }
                else
                {
                    ValidationMessage = "Selected record could not be found - it may have been deleted.";
                }
            }
        }

        public IActionResult OnPostSave()
        {
            LoadDropdowns();

            var subjectBLL = new SubjectBLL(_configuration);
            var secondaryPerson = subjectBLL.GetPerson(AddLink.FISecondaryCode);
            var secondaryInst = subjectBLL.GetInstitution(AddLink.FISecondaryCode);
            AddLink.FISecondaryName = secondaryPerson?.Name ?? secondaryInst?.TradeName ?? "Not found";

            ValidationMessage = ValidateContractLink(AddLink);

            if (!string.IsNullOrEmpty(ValidationMessage))
            {
                LoadLinkList();
                return Page();
            }

            AddLink.RecordType = "L";
            AddLink.FICode = "046";
            AddLink.MakeBy = User.Identity.Name;
            AddLink.MakeDate = DateTime.Now;
            AddLink.RecordStatus = "C";

            var contractBLL = new ContractBLL(_configuration);
            bool success = contractBLL.SaveContractLink(AddLink);

            if (!success)
            {
                ValidationMessage = "Save failed. Please check the values and try again.";
                LoadLinkList();
                return Page();
            }

            return RedirectToPage("/Contract/ContractLinks");
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedLinks == null || !SelectedLinks.Any())
            {
                DeleteMessage = "Please select at least one record to delete.";
                LoadLinkList();
                return Page();
            }

            var toDelete = new List<ContractLink>();
            foreach (var item in SelectedLinks)
            {
               
                var parts = item.Split('|');
                if (parts.Length == 2)
                {
                    toDelete.Add(new ContractLink
                    {
                        FIContractCode = parts[0],
                        FISecondaryCode = parts[1],
                        MakeBy = User.Identity.Name,
                        MakeDate = DateTime.Now
                    });
                }
            }

            var contractBLL = new ContractBLL(_configuration);
            bool success = contractBLL.FlagDeleteContractLinks(toDelete);

            if (!success)
            {
                DeleteMessage = "Delete failed. Please try again.";
            }

            LoadLinkList();
            return Page();
        }

        private string ValidateContractLink(ContractLink link)
        {
            var v = new ContractLinkValidator();
            var res = v.Validate(link);
            if (!res.IsValid)
            {
                return "- " + string.Join("<br/>- ", res.Errors.Select(e => e.ErrorMessage).Distinct());
            }
            else
            {
                return string.Empty;
            }
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            LinkTypeList = _lovBLL.GetListOfValues(LovTypes.LINK_TYPES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
        }

        private void LoadLinkList()
        {
            var contractBLL = new ContractBLL(_configuration);
            var allResults = contractBLL.GetContractLinksFiltered(
                SearchBranchCode, SearchPrimaryCode, SearchPrimaryName,
                SearchSecondaryCode, SearchSecondaryName, SearchContractCode);

            TotalCount = allResults.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (TotalPages < 1) TotalPages = 1;
            if (PageIndex < 1) PageIndex = 1;
            if (PageIndex > TotalPages) PageIndex = TotalPages;

            LinkList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}