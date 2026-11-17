using CIBnext.BLL;
using CIBnext.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Contract
{
    [Authorize(Roles = "3002,ADMIN")]
    public class NonInstallmentContractErrorListModel : PageModel
    {
        private readonly ContractBLL _contractBLL;
        private readonly ListOfValuesBLL _lovBLL;

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
            ErrorList = _contractBLL.GetNonInstalmentContractError(
                BranchCode ?? "",
                ContractCode ?? "",
                FISubjectCode ?? ""
            ) ?? new List<ContractError>();
        }
    }
}
