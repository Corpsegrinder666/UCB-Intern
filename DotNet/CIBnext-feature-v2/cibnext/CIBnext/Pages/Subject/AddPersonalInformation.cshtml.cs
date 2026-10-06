using CIBnext.BLL;
using CIBnext.DAL;
using CIBnext.DAO;
using CIBnext.Validators;
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
    public class AddPersonalInformationModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;

        public AddPersonalInformationModel(ListOfValuesBLL lovBLL, IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        [BindProperty]
        public PersonalData Subject { get; set; } = new PersonalData();
        public string ValidationMessage { get; set; }
        public bool IsEdit { get; set; }
        public List<SelectListItem> BranchCodeList { get; set; }
        public List<SelectListItem> GenderList { get; set; }
        public List<SelectListItem> CountryList { get; set; }
        public List<SelectListItem> SectorTypeList { get; set; }
        public List<SelectListItem> SectorCodeList { get; set; }
        public List<SelectListItem> IdTypeList { get; set; }
        public List<SelectListItem> DistrictList { get; set; }

        private void LoadDropdowns()
        {
            BranchCodeList = _lovBLL.GetListOfValues(LovTypes.BRANCHES)
    .Select(x => new SelectListItem
    {
        Value = x.Key,
        Text = x.Value
    }).ToList();
            GenderList = _lovBLL.GetListOfValues(LovTypes.GENDER)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            CountryList = _lovBLL.GetListOfValues(LovTypes.COUNTRIES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            SectorTypeList = _lovBLL.GetListOfValues(LovTypes.SECTOR_TYPES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            SectorCodeList = _lovBLL.GetListOfValues(LovTypes.SECTOR_CODES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            IdTypeList = _lovBLL.GetListOfValues(LovTypes.ID_TYPES)
                .Select(x => new SelectListItem { Value = x.Key, Text = x.Value }).ToList();
            DistrictList = _lovBLL.GetDistrictsByCountryCode("BD")
                .Select(x => new SelectListItem { Value = x, Text = x }).ToList();
        }

        public void OnGet(string fiSubjectCode)
        {
            LoadDropdowns();

            if (!string.IsNullOrEmpty(fiSubjectCode))
            {
                
                var subjectBLL = new SubjectBLL(_configuration);
                var existing = subjectBLL.GetPerson(fiSubjectCode);

                if (existing != null)
                {
                    Subject = existing;
                    IsEdit = true;
                }
                else
                {
                    ValidationMessage = "Person not found for subject code: " + fiSubjectCode;
                }
            }

            if (Subject.PermanentAddress == null) Subject.PermanentAddress = new Address();
            if (Subject.PresentAddress == null) Subject.PresentAddress = new Address();
            if (Subject.BusinessAddress == null) Subject.BusinessAddress = new Address();

            if (!IsEdit)
                Subject.MakeDate = DateTime.Now;
        }

        public IActionResult OnPost()
        {
            LoadDropdowns();

            IsEdit = !string.IsNullOrEmpty(Subject.FISubjectCode);

            ModelState.Clear();

            ValidationMessage = ValidatePerson(Subject);

            if (!string.IsNullOrEmpty(ValidationMessage))
            {
                return Page();
            }

            Subject.RecordType = "P";
            Subject.FICode = "046";
            Subject.MakeBy = User.Identity.Name;
            Subject.MakeDate = DateTime.Now;
            Subject.RecordStatus = "C";

            var subjectDAL = new SubjectDAL(_configuration);
            string newSubjectCode = subjectDAL.InsertPersonalData(Subject);

            if (string.IsNullOrEmpty(newSubjectCode))
            {
                
                return Page();
            }

            return RedirectToPage("/Subject/PersonalInformation");
        }
        private string ValidatePerson(PersonalData personal)
        {
            var pdv = new PersonalDataValidator();
            var res = pdv.Validate(personal);
            if (!res.IsValid)
            {
                return "- " + string.Join("<br/>- ", res.Errors.Select(e => e.ErrorMessage).Distinct());
            }
            else
            {
                return string.Empty;
            }
        }
    }
}