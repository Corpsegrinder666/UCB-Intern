using CIBnext.BLL;
using CIBnext.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Subject
{
    [Authorize(Roles = "2001,ADMIN")]
    public class PersonalInformationErrorListModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;

        public PersonalInformationErrorListModel(
            ListOfValuesBLL lovBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public string BranchCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SubjectCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SubjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<SubjectError> ErrorList { get; set; }
            = new List<SubjectError>();

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
            var subjectBLL = new SubjectBLL(_configuration);

            ErrorList = subjectBLL.GetPersonalDataError(
                BranchCode,
                SubjectCode,
                SubjectName
            ) ?? new List<SubjectError>();
        }
    }
}
