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

        // =========================
        // Search filters
        // =========================

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

        // Determines whether Search was actually pressed.
        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        // =========================
        // Delete
        // =========================

        [BindProperty]
        public List<string> SelectedCodes { get; set; }
            = new List<string>();

        // =========================
        // Messages
        // =========================

        public string SearchMessage { get; set; }

        public string DeleteMessage { get; set; }

        public string DeleteMessageType { get; set; } = "warning";

        public string DeleteMessageTitle { get; set; } = "Delete";

        [TempData]
        public string SuccessMessage { get; set; }

        // =========================
        // Dropdowns
        // =========================

        public List<SelectListItem> BranchCodeList { get; set; }

        public List<SelectListItem> ContractPhaseList { get; set; }

        // =========================
        // Contract list
        // =========================

        public List<NonInstalmentContract> ContractList { get; set; }
            = new List<NonInstalmentContract>();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        // =========================
        // GET
        // =========================

        public void OnGet()
        {
            LoadDropdowns();

            /*
             * Show the empty-search message only when the
             * Search button was actually pressed.
             *
             * Initial page load should not show the message.
             */
            if (SearchSubmitted && IsSearchEmpty())
            {
                SearchMessage =
                    "Please enter at least one search criteria.";
            }

            LoadContractList();
        }

        // =========================
        // DELETE
        // =========================

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                DeleteMessage =
                    "Please select at least one record to delete.";

                DeleteMessageType = "warning";
                DeleteMessageTitle = "Selection Required";

                LoadContractList();

                return Page();
            }

            try
            {
                var toDelete = SelectedCodes
                    .Select(code => new DAO.Contract
                    {
                        FIContractCode = code,
                        MakeBy = User.Identity?.Name,
                        MakeDate = DateTime.Now
                    })
                    .ToList();

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
            catch
            {
                DeleteMessage =
                    "An error occurred while deleting the selected contract(s). Please try again.";

                DeleteMessageType = "error";
                DeleteMessageTitle = "Error";
            }

            LoadContractList();

            return Page();
        }

        // =========================
        // SEARCH CHECK
        // =========================

        private bool IsSearchEmpty()
        {
            return string.IsNullOrWhiteSpace(BranchCode)
                && string.IsNullOrWhiteSpace(ContractCode)
                && string.IsNullOrWhiteSpace(FISubjectCode)
                && string.IsNullOrWhiteSpace(ContractPhase)
                && string.IsNullOrWhiteSpace(UbsAcNo);
        }

        // =========================
        // DROPDOWNS
        // =========================

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

        // =========================
        // CONTRACT LIST
        // =========================

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

            TotalPages =
                (int)Math.Ceiling(
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
