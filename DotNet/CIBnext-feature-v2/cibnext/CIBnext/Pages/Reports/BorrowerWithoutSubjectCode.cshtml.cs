using CIBnext.BLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Reports
{
    [Authorize(Roles = "5003,ADMIN")]
    public class BorrowerWithoutSubjectCodeInfoModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;

        public BorrowerWithoutSubjectCodeInfoModel(ListOfValuesBLL lovBLL)
        {
            _lovBLL = lovBLL;
        }

        [BindProperty(SupportsGet = true)]
        public string BranchCode { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? LoanListDate { get; set; }

        public string ValidationMessage { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
        }

        public IActionResult OnPostDownload()
        {
            LoadDropdowns();

            if (LoanListDate == null)
            {
                ValidationMessage = "Loan List Date is required.";
                return Page();
            }

            // TODO: MisBLL.GetLatestCifs + GetLoansForCifs + SubjectBLL.GetSubjectByCifNo
            // pdf logic
            return Page();
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
        }
    }
}


