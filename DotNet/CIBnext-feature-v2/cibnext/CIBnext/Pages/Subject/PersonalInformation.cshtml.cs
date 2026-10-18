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

        public PersonalInformationModel(
            ListOfValuesBLL lovBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public PersonalData Subject { get; set; } = new PersonalData();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        [BindProperty]
        public List<string> SelectedCodes { get; set; } = new List<string>();

        // Popup message and style.
        public string PopupMessage { get; set; } = string.Empty;

        public string PopupType { get; set; } = "error";

        // Reads the success message set by AddPersonalInformation
        // before it redirects to this page.
        [TempData]
        public string SuccessMessage { get; set; }

        // Retained for compatibility with existing page code, if needed.
        public string DeleteMessage { get; set; }

        public string ValidationMessage { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<PersonalData> PersonList { get; set; }
            = new List<PersonalData>();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
            LoadPersonList();

            // Do not show a no-results popup on the initial page load.
            // Show it only when the user has submitted a search.
            if (SearchSubmitted && TotalCount == 0)
            {
                PopupMessage =
                    "No person found matching the provided search information.";

                PopupType = "warning";
            }
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            // Validate that at least one record was selected.
            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                PopupMessage =
                    "Please select at least one record to delete.";

                PopupType = "warning";

                LoadPersonList();
                return Page();
            }

            try
            {
                var toDelete = SelectedCodes
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .Select(code => new PersonalData
                    {
                        FISubjectCode = code,
                        MakeBy = User.Identity?.Name ?? "SYSTEM",
                        MakeDate = DateTime.Now
                    })
                    .ToList();

                if (!toDelete.Any())
                {
                    PopupMessage =
                        "Please select at least one valid record to delete.";

                    PopupType = "warning";

                    LoadPersonList();
                    return Page();
                }

                var subjectBLL = new SubjectBLL(_configuration);

                bool success = subjectBLL.FlagDeletePersons(toDelete);

                if (success)
                {
                    PopupMessage =
                        toDelete.Count == 1
                            ? "The selected record was deleted successfully."
                            : "The selected records were deleted successfully.";

                    PopupType = "success";
                }
                else
                {
                    PopupMessage =
                        "Delete failed. Please try again.";

                    PopupType = "error";
                }
            }
            catch (Exception)
            {
                PopupMessage =
                    "An unexpected error occurred while deleting the selected records. Please try again.";

                PopupType = "error";
            }

            // Reload the list using the submitted search filters.
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
                })
                .ToList();
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

            if (allResults == null)
            {
                allResults = new List<PersonalData>();
            }

            TotalCount = allResults.Count;

            TotalPages = (int)Math.Ceiling(
                TotalCount / (double)PageSize);

            if (TotalPages < 1)
            {
                TotalPages = 1;
            }

            if (PageIndex < 1)
            {
                PageIndex = 1;
            }

            if (PageIndex > TotalPages)
            {
                PageIndex = TotalPages;
            }

            PersonList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
