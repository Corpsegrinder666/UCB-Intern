using CIBnext.BLL;
using CIBnext.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace CIBnext.Pages.Contract
{
    [Authorize(Roles = "3001,ADMIN")]
    public class InstallmentContractErrorListModel : PageModel
    {
        private readonly ContractBLL _contractBLL;

        public InstallmentContractErrorListModel(IConfiguration configuration)
        {
            _contractBLL = new ContractBLL(configuration);
        }

        public List<ContractError> Errors { get; set; } = new List<ContractError>();

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

        public void OnGet()
        {
            Errors = _contractBLL.GetInstalmentContractError(
                BranchCode,
                ContractCode,
                FISubjectCode
            ) ?? new List<ContractError>();
        }
    }
}
