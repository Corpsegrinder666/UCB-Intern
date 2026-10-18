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
    [Authorize(Roles = "2002,ADMIN")]
    public class AddInstitutionInformationModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly MiscBLL _miscBll;
        private readonly IConfiguration _configuration;

        public AddInstitutionInformationModel(
            ListOfValuesBLL lovBLL,
            MiscBLL miscBll,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _miscBll = miscBll;
            _configuration = configuration;
        }

        [BindProperty]
        public Institution Subject { get; set; } = new Institution();

        public string ValidationMessage { get; set; } = string.Empty;
        public bool IsEdit { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> CountryList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> SectorTypeList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> SectorCodeList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> LegalFormList { get; set; }
            = new List<SelectListItem>();

        public List<SelectListItem> DistrictList { get; set; }
            = new List<SelectListItem>();

        private void LoadDropdowns()
        {
            BranchCodeList = _miscBll.GetBranches().Result
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.BranchName
                })
                .ToList();

            CountryList = _lovBLL.GetListOfValues(LovTypes.COUNTRIES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            SectorTypeList = _lovBLL.GetListOfValues(LovTypes.SECTOR_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            SectorCodeList = _lovBLL.GetListOfValues(LovTypes.SECTOR_CODES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            LegalFormList = _lovBLL.GetListOfValues(LovTypes.INSTITUTION_LEGAL_FORMS)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            DistrictList = _lovBLL.GetDistrictsByCountryCode("BD")
                .Select(x => new SelectListItem
                {
                    Value = x,
                    Text = x
                })
                .ToList();
        }

        public void OnGet(string fiSubjectCode)
        {
            LoadDropdowns();

            if (!string.IsNullOrWhiteSpace(fiSubjectCode))
            {
                var subjectBLL = new SubjectBLL(_configuration);
                var existing = subjectBLL.GetInstitution(fiSubjectCode);

                if (existing != null)
                {
                    Subject = existing;
                    IsEdit = true;
                }
                else
                {
                    ValidationMessage =
                        "No institution was found for the provided subject code.";
                }
            }

            if (Subject.BusinessAddress == null)
                Subject.BusinessAddress = new Address();

            if (Subject.FactoryAddress == null)
                Subject.FactoryAddress = new Address();

            if (!IsEdit)
                Subject.MakeDate = DateTime.Now;
        }

        public IActionResult OnPost()
        {
            LoadDropdowns();

            IsEdit = !string.IsNullOrWhiteSpace(Subject.FISubjectCode);

            if (Subject.BusinessAddress == null)
                Subject.BusinessAddress = new Address();

            if (Subject.FactoryAddress == null)
                Subject.FactoryAddress = new Address();

            ModelState.Clear();

            ValidationMessage = ValidateInstitution(Subject);

            if (!string.IsNullOrWhiteSpace(ValidationMessage))
            {
                return Page();
            }

            try
            {
                Subject.RecordType = "I";
                Subject.FICode = "046";
                Subject.MakeBy = User.Identity?.Name ?? "SYSTEM";
                Subject.MakeDate = DateTime.Now;
                Subject.RecordStatus = "C";

                var subjectDAL = new SubjectDAL(_configuration);
                string newSubjectCode = subjectDAL.InsertInstitution(Subject);

                if (string.IsNullOrWhiteSpace(newSubjectCode))
                {
                    ValidationMessage =
                        "The institution could not be saved. Please check the information and try again.";

                    return Page();
                }

                TempData["SuccessMessage"] =
                    $"Institution saved successfully. Subject code: {newSubjectCode}";

                return RedirectToPage("/Subject/InstitutionInformation");
            }
            catch (Exception)
            {
                ValidationMessage =
                    "An unexpected error occurred while saving the institution. Please try again.";

                return Page();
            }
        }

        private string ValidateInstitution(Institution institution)
        {
            var validator = new InstitutionValidator();
            var result = validator.Validate(institution);

            if (!result.IsValid)
            {
                return string.Join(
                    Environment.NewLine,
                    result.Errors
                        .Select(error => "- " + error.ErrorMessage)
                        .Distinct());
            }

            return string.Empty;
        }
    }
}
