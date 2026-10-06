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

namespace CIBnext.Pages.Subject
{
    [Authorize(Roles = "2001,ADMIN")]
    public class PersonalInformationModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;
        private const int PageSize = 10;

        public PersonalInformationModel(ListOfValuesBLL lovBLL, IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public PersonalData Subject { get; set; } = new PersonalData();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty]
        public List<string> SelectedCodes { get; set; } = new List<string>();

        public string DeleteMessage { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }
        public List<PersonalData> PersonList { get; set; } = new List<PersonalData>();
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
            LoadPersonList();
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                DeleteMessage = "Please select at least one record to delete.";
                LoadPersonList();
                return Page();
            }

            var toDelete = SelectedCodes
                .Select(code => new PersonalData
                {
                    FISubjectCode = code,
                    MakeBy = User.Identity.Name,
                    MakeDate = DateTime.Now
                })
                .ToList();

            var subjectBLL = new SubjectBLL(_configuration);
            bool success = subjectBLL.FlagDeletePersons(toDelete);

            if (!success)
            {
                DeleteMessage = "Delete failed. Please try again.";
            }

            LoadPersonList();
            return Page();
        }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                }).ToList();
        }

        private void LoadPersonList()
        {
            var subjectBLL = new SubjectBLL(_configuration);
            var allResults = subjectBLL.GetFilteredPersonList(
                Subject.BranchCode,
                Subject.FISubjectCode,
                Subject.Name,
                Subject.FathersName,
                Subject.DateOfBirth,
                Subject.CifNo);

            TotalCount = allResults.Count;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);
            if (TotalPages < 1) TotalPages = 1;
            if (PageIndex < 1) PageIndex = 1;
            if (PageIndex > TotalPages) PageIndex = TotalPages;

            PersonList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}