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
        private readonly MiscBLL _miscBLL;
        private readonly IConfiguration _configuration;

        private const int PageSize = 10;

        public ContractLinksModel(
            ListOfValuesBLL lovBLL,
            MiscBLL miscBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _miscBLL = miscBLL;
            _configuration = configuration;
        }

        // Search filters
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

        // Edit
        [BindProperty(SupportsGet = true)]
        public string EditContract { get; set; }

        [BindProperty(SupportsGet = true)]
        public string EditSecondary { get; set; }

        public bool IsEdit { get; set; }

        // Add/Edit contract link
        [BindProperty]
        public ContractLink AddLink { get; set; } = new ContractLink();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedLinks { get; set; } = new List<string>();

        // Popup messages
        public string ValidationMessage { get; set; }
        public string ValidationMessageType { get; set; }
        public string ValidationMessageTitle { get; set; }

        public string DeleteMessage { get; set; }
        public string DeleteMessageType { get; set; }
        public string DeleteMessageTitle { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        // Search dropdown: branch code
        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        // Add Contract Link dropdown: branch name
        public List<SelectListItem> BranchNameList { get; set; }
            = new List<SelectListItem>();

        // Link type dropdown
        public List<SelectListItem> LinkTypeList { get; set; }
            = new List<SelectListItem>();

        public List<ContractLink> LinkList { get; set; }
            = new List<ContractLink>();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();

            // Detect whether the Search form was submitted.
            // The Clear link does not include any search query keys.
            bool searchSubmitted =
                Request.Query.ContainsKey("SearchBranchCode") ||
                Request.Query.ContainsKey("SearchPrimaryCode") ||
                Request.Query.ContainsKey("SearchPrimaryName") ||
                Request.Query.ContainsKey("SearchSecondaryCode") ||
                Request.Query.ContainsKey("SearchSecondaryName") ||
                Request.Query.ContainsKey("SearchContractCode");

            bool hasSearchCriteria =
                !string.IsNullOrWhiteSpace(SearchBranchCode) ||
                !string.IsNullOrWhiteSpace(SearchPrimaryCode) ||
                !string.IsNullOrWhiteSpace(SearchPrimaryName) ||
                !string.IsNullOrWhiteSpace(SearchSecondaryCode) ||
                !string.IsNullOrWhiteSpace(SearchSecondaryName) ||
                !string.IsNullOrWhiteSpace(SearchContractCode);

            if (searchSubmitted && !hasSearchCriteria)
            {
                ValidationMessage =
                    "Please provide at least one search option.";

                ValidationMessageType = "warning";
                ValidationMessageTitle = "Search Required";
            }

            LoadLinkList();

            // Load the selected record for editing.
            if (!string.IsNullOrEmpty(EditContract))
            {
                var contractBLL = new ContractBLL(_configuration);

                var all = contractBLL.GetContractLinksFiltered(
                    null, null, null, null, null, null);

                var existing = all.FirstOrDefault(l =>
                    l.FIContractCode == EditContract &&
                    (l.FISecondaryCode ?? string.Empty) ==
                    (EditSecondary ?? string.Empty));

                if (existing != null)
                {
                    AddLink = existing;
                    IsEdit = true;
                }
                else
                {
                    ValidationMessage =
                        "Selected record could not be found - it may have been deleted.";

                    ValidationMessageType = "warning";
                    ValidationMessageTitle = "Record Not Found";
                }
            }
        }

        public IActionResult OnPostSave()
        {
            LoadDropdowns();

            var subjectBLL = new SubjectBLL(_configuration);

            var secondaryPerson =
                subjectBLL.GetPerson(AddLink.FISecondaryCode);

            var secondaryInst =
                subjectBLL.GetInstitution(AddLink.FISecondaryCode);

            AddLink.FISecondaryName =
                secondaryPerson?.Name ??
                secondaryInst?.TradeName ??
                "Not found";

            ValidationMessage = ValidateContractLink(AddLink);

            if (!string.IsNullOrEmpty(ValidationMessage))
            {
                ValidationMessageType = "warning";
                ValidationMessageTitle = "Validation Required";

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
                ValidationMessage =
                    "Save failed. Please check the values and try again.";

                ValidationMessageType = "error";
                ValidationMessageTitle = "Save Failed";

                LoadLinkList();
                return Page();
            }

            SuccessMessage = "Contract link saved successfully.";

            return RedirectToPage("/Contract/ContractLinks");
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedLinks == null || !SelectedLinks.Any())
            {
                DeleteMessage =
                    "Please select at least one record to delete.";

                DeleteMessageType = "warning";
                DeleteMessageTitle = "Selection Required";

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

            bool success =
                contractBLL.FlagDeleteContractLinks(toDelete);

            if (!success)
            {
                DeleteMessage = "Delete failed. Please try again.";

                DeleteMessageType = "error";
                DeleteMessageTitle = "Delete Failed";
            }
            else
            {
                DeleteMessage =
                    "Selected record(s) deleted successfully.";

                DeleteMessageType = "success";
                DeleteMessageTitle = "Success";
            }

            LoadLinkList();
            return Page();
        }

        private string ValidateContractLink(ContractLink link)
        {
            var validator = new ContractLinkValidator();
            var result = validator.Validate(link);

            if (!result.IsValid)
            {
                return "- " + string.Join(
                    "<br/>- ",
                    result.Errors
                        .Select(e => e.ErrorMessage)
                        .Distinct());
            }

            return string.Empty;
        }

        private void LoadDropdowns()
        {
            var branches = _miscBLL.GetBranches().Result;

            // Search Option: display branch codes.
            BranchCodeList = branches
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.CibBranchCode
                })
                .ToList();

            // Add/Edit Contract Link: display branch names.
            // Keep the selected value as the regular branch code.
            BranchNameList = branches
                .Select(x => new SelectListItem
                {
                    Value = x.BranchCode,
                    Text = x.BranchName
                })
                .ToList();

            LinkTypeList = _lovBLL
                .GetListOfValues(LovTypes.LINK_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();
        }

        private void LoadLinkList()
        {
            var contractBLL = new ContractBLL(_configuration);

            var allResults = contractBLL.GetContractLinksFiltered(
                SearchBranchCode,
                SearchPrimaryCode,
                SearchPrimaryName,
                SearchSecondaryCode,
                SearchSecondaryName,
                SearchContractCode);

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
