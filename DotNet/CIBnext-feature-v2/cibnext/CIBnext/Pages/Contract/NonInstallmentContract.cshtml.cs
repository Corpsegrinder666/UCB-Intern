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

        public NonInstallmentContractModel(ListOfValuesBLL lovBLL, IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        // Search filter
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

        [BindProperty]
        public List<string> SelectedCodes { get; set; } = new List<string>();

        public string DeleteMessage { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> ContractPhaseList { get; set; }
        public List<NonInstalmentContract> ContractList { get; set; } = new List<NonInstalmentContract>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
            LoadContractList();
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                DeleteMessage = "Please select at least one record to delete.";
                LoadContractList();
                return Page();
            }

            var toDelete = SelectedCodes
                .Select(code => new DAO.Contract
                {
                    FIContractCode = code,
                    MakeBy = User.Identity.Name,
                    MakeDate = DateTime.Now
                })
                .ToList();

            var contractDAL = new ContractDAL(_configuration);
            bool success = contractDAL.FlagDeleteNonInstalmentContracts(toDelete);

            if (!success)
            {
                DeleteMessage = "Delete failed. Please try again.";
            }

            LoadContractList();
            return Page();
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            ContractPhaseList = _lovBLL.GetListOfValues(LovTypes.CONTRACT_PHASES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
        }

        private void LoadContractList()
        {
            var contractDAL = new ContractDAL(_configuration);
            var allResults = contractDAL.SelectNonInstalmentContractsWithFilters(
                FISubjectCode, BranchCode, ContractCode, ContractPhase, UbsAcNo);

            TotalCount = allResults.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (TotalPages < 1) TotalPages = 1;
            if (PageIndex < 1) PageIndex = 1;
            if (PageIndex > TotalPages) PageIndex = TotalPages;

            ContractList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}