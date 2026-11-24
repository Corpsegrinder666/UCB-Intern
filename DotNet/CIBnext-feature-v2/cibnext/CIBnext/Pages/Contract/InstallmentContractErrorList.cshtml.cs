using CIBnext.BLL;
using CIBnext.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Contract
{
    [Authorize(Roles = "3001,ADMIN")]
    public class InstallmentContractErrorListModel : PageModel
    {
        private readonly ContractBLL _contractBLL;

        private const int PageSize = 15;

        public InstallmentContractErrorListModel(IConfiguration configuration)
        {
            _contractBLL = new ContractBLL(configuration);
        }

        public List<ContractError> Errors { get; set; }
            = new List<ContractError>();

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
        public bool SearchSubmitted { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public void OnGet()
        {
            if (SearchSubmitted)
            {
                LoadErrors();
            }
            else
            {
                ClearErrors();
            }
        }

        private void LoadErrors()
        {
            var allErrors = _contractBLL.GetInstalmentContractError(
                BranchCode,
                ContractCode,
                FISubjectCode
            ) ?? new List<ContractError>();

            TotalCount = allErrors.Count;

            TotalPages = (int)Math.Ceiling(
                TotalCount / (double)PageSize);

            if (PageNumber < 1)
            {
                PageNumber = 1;
            }

            if (PageNumber > TotalPages && TotalPages > 0)
            {
                PageNumber = TotalPages;
            }

            Errors = allErrors
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }

        private void ClearErrors()
        {
            Errors = new List<ContractError>();
            TotalCount = 0;
            TotalPages = 0;
            PageNumber = 1;
        }
    }
}
