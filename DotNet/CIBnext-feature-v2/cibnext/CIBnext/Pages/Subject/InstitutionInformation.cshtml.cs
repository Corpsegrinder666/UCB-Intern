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
    [Authorize(Roles = "2002,ADMIN")]
    public class InstitutionInformationModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly IConfiguration _configuration;
        private const int PageSize = 10;

        public InstitutionInformationModel(
            ListOfValuesBLL lovBLL,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _configuration = configuration;
        }

        [BindProperty(SupportsGet = true)]
        public Institution Subject { get; set; } = new Institution();

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public bool SearchSubmitted { get; set; }

        [BindProperty]
        public List<string> SelectedCodes { get; set; }
            = new List<string>();

        public string DeleteMessage { get; set; } = string.Empty;

        public string PopupMessage { get; set; } = string.Empty;

        public string PopupType { get; set; } = "error";

        [TempData]
        public string SuccessMessage { get; set; }

        public List<SelectListItem> BranchCodeList { get; set; }
            = new List<SelectListItem>();

        public List<Institution> InstitutionList { get; set; }
            = new List<Institution>();

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public void OnGet()
        {
            LoadDropdowns();
            LoadInstitutionList();

            // Only warn when a submitted search returns no results.
            if (SearchSubmitted && TotalCount == 0)
            {
                PopupMessage =
                    "No institution found matching the provided search information.";

                PopupType = "warning";
            }
        }

        public IActionResult OnPostDelete()
        {
            LoadDropdowns();

            if (SelectedCodes == null || !SelectedCodes.Any())
            {
                PopupMessage =
                    "Please select at least one record to delete.";

                PopupType = "warning";

                LoadInstitutionList();
                return Page();
            }

            try
            {
                var toDelete = SelectedCodes
                    .Where(code => !string.IsNullOrWhiteSpace(code))
                    .Distinct()
                    .Select(code => new Institution
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

                    LoadInstitutionList();
                    return Page();
                }

                var subjectBLL = new SubjectBLL(_configuration);

                bool success =
                    subjectBLL.FlagDeleteInstitutions(toDelete);

                if (success)
                {
                    PopupMessage = toDelete.Count == 1
                        ? "The selected institution was deleted successfully."
                        : "The selected institutions were deleted successfully.";

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
                    "An unexpected error occurred while deleting the selected institutions. Please try again.";

                PopupType = "error";
            }

            LoadInstitutionList();

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

        private void LoadInstitutionList()
        {
            var subjectBLL = new SubjectBLL(_configuration);

            var allResults = subjectBLL.GetInstitutionsByFilter(
                Subject.BranchCode,
                Subject.FISubjectCode,
                Subject.TradeName,
                Subject.BusinessAddress != null
                    ? Subject.BusinessAddress.Street
                    : null,
                Subject.CifNo);

            if (allResults == null)
            {
                allResults = new List<Institution>();
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

            InstitutionList = allResults
                .Skip((PageIndex - 1) * PageSize)
                .Take(PageSize)
                .ToList();
        }
    }
}
