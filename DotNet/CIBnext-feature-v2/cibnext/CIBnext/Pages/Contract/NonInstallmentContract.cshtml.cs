using CIBnext.BLL;
using CIBnext.DAL;
using CIBnext.DAO;
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
    [Authorize(Roles = "3002,ADMIN")]
    public class NonInstallmentContractModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;

        private const int PageSize = 10;

        public NonInstallmentContractModel(
            ListOfValuesBLL lovBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        // Search filters
        [BindProperty(SupportsGet = true)]
        public string BranchCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ContractCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FISubjectCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ContractPhase { get; set; }

        [BindProperty(SupportsGet = true)]
        public string UbsAcNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        // True only after Search has been submitted.
        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        // Delete
        [BindProperty]
        public List<string> SelectedCodes { get; set; }
            = new List<string>();

        // Messages
        public string DeleteMessage { get; set; } = string.Empty;

        public string DeleteMessageType { get; set; } = "warning";

        public string DeleteMessageTitle { get; set; } = "Delete";

        [TempData]
        public string SuccessMessage { get; set; }

        // Dropdowns
        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> ContractPhaseList { get; set; }
            = new List<SelectListItem>();

        // Contract list
        public List<NonInstalmentContract> ContractList { get; set; }
            = new List<NonInstalmentContract>();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        // GET
        public void OnGet()
        {
            LoadDropdowns();

            if (SearchSubmitted)
            {
                LoadContractList();
            }
            else
            {
                ClearContractList();
            }
        }

        // DELETE
        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                DeleteMessage =
                    "Please select at least one record to delete.";

                DeleteMessageType = "warning";
                DeleteMessageTitle = "Selection Required";

                RestoreSearchResults();

                return Page();
            }

            try
            {
                var toDelete = SelectedCodes
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Select(code => new DAO.Contract
                    {
                        FIContractCode = code,
                        MakeBy = User.Identity?.Name,
                        MakeDate = DateTime.Now
                    })
                    .ToList();

                if (!toDelete.Any())
                {
                    DeleteMessage =
                        "No valid records were selected. Please select the records again.";

                    DeleteMessageType = "warning";
                    DeleteMessageTitle = "Invalid Selection";

                    RestoreSearchResults();

                    return Page();
                }

                var contractDAL = new ContractDAL(_configuration);

                bool success =
                    contractDAL.FlagDeleteNonInstalmentContracts(toDelete);

                if (success)
                {
                    DeleteMessage = toDelete.Count == 1
                        ? "Contract deleted successfully."
                        : toDelete.Count + " contracts deleted successfully.";

                    DeleteMessageType = "success";
                    DeleteMessageTitle = "Success";
                }
                else
                {
                    DeleteMessage =
                        "Delete failed. Please try again.";

                    DeleteMessageType = "error";
                    DeleteMessageTitle = "Delete Failed";
                }
            }
            catch (Exception)
            {
                DeleteMessage =
                    "An error occurred while deleting the selected contract(s). Please try again.";

                DeleteMessageType = "error";
                DeleteMessageTitle = "Error";
            }

            RestoreSearchResults();

            return Page();
        }

        // DROPDOWNS
        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL
                .GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            ContractPhaseList = _lovBLL
                .GetListOfValues(LovTypes.CONTRACT_PHASES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();
        }

        // RESTORE SEARCH RESULTS AFTER POST
        private void RestoreSearchResults()
        {
            if (SearchSubmitted)
            {
                LoadContractList();
            }
            else
            {
                ClearContractList();
            }
        }

        // CLEAR RESULTS
        private void ClearContractList()
        {
            ContractList = new List<NonInstalmentContract>();
            TotalCount = 0;
            TotalPages = 0;
            PageIndex = 1;
        }

        // CONTRACT LIST
        private void LoadContractList()
        {
            var contractDAL = new ContractDAL(_configuration);

            var allResults =
                contractDAL.SelectNonInstalmentContractsWithFilters(
                    FISubjectCode,
                    BranchCode,
                    ContractCode,
                    ContractPhase,
                    UbsAcNo);

            if (allResults == null)
            {
                allResults = new List<NonInstalmentContract>();
            }

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

            ContractList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
