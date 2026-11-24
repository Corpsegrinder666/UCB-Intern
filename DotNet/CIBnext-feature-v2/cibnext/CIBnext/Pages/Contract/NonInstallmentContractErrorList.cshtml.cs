using CIBnext.BLL;
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
    public class NonInstallmentContractErrorListModel : PageModel
    {
        private readonly ContractBLL _contractBLL;
        private readonly ListOfValuesBLL _lovBLL;

        private const int PageSize = 15;

        public NonInstallmentContractErrorListModel(
            IConfiguration configuration,
            ListOfValuesBLL lovBLL)
        {
            _contractBLL = new ContractBLL(configuration);
            _lovBLL = lovBLL;
        }

        [BindProperty(SupportsGet = true)]
        public string? BranchCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ContractCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FISubjectCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ContractPhase { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? UbsAcNo { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<ContractError> ErrorList { get; set; }
            = new List<ContractError>();

        public void OnGet()
        {
            LoadDropdowns();

            if (SearchSubmitted)
            {
                LoadErrorList();
            }
            else
            {
                ClearErrorList();
            }
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();
        }

        private void LoadErrorList()
        {
            var allErrors = _contractBLL.GetNonInstalmentContractError(
                BranchCode ?? "",
                ContractCode ?? "",
                FISubjectCode ?? ""
            ) ?? new List<ContractError>();

            TotalCount = allErrors.Count;

            TotalPages = (int)Math.Ceiling(
                TotalCount / (double)PageSize);

            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (TotalPages > 0 && PageNumber > TotalPages)
            {
                PageNumber = TotalPages;
            }

            ErrorList = allErrors
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        private void ClearErrorList()
        {
            ErrorList = new List<ContractError>();
            TotalCount = 0;
            TotalPages = 0;
            PageNumber = 1;
        }
    }
}
