
using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CIBnext.Validators
{
    public class PersonalDataValidator : AbstractValidator<PersonalData>
    {
        public PersonalDataValidator()
        {
            var nidRegex = new Regex(@"^(\d{17})$");
            var sidRegex = new Regex(@"^(\d{10})$");

            RuleFor(p => p.BranchCode).NotEmpty().WithMessage("Branch code is required");

            RuleFor(p => p.SectorCode).NotEqual(0).When(p => p.SectorType != 0).WithMessage("Sector Type and Sector Code must be filled togather.");
            RuleFor(p => p.SectorType).NotEqual(0).When(p => p.SectorCode != 0).WithMessage("Sector Type and Sector Code must be filled togather.");

            RuleFor(p => p.Gender).NotEmpty().WithMessage("Gender is Required");

            RuleFor(p => p.PresentAddress).NotNull()
                .WithMessage("Present Address is required");

            RuleFor(p => p.PresentAddress.Street).NotEmpty()
                .When(p => p.PresentAddress != null)
                .WithMessage("Present address street is required.");

            RuleFor(p => p.PresentAddress.District).NotEmpty()
                .When(p => p.PresentAddress != null)
                .WithMessage("Present address district is required.");

            RuleFor(p => p.PresentAddress.CountryCode).NotEmpty()
                .When(p => p.PresentAddress != null)
                .WithMessage("Present address country is required.");

            RuleFor(p => p.PermanentAddress).NotNull()
                .WithMessage("Permanent Address is required");

            RuleFor(p => p.PermanentAddress.Street).NotEmpty()
                .When(p => p.PermanentAddress != null)
                .WithMessage("Present address street is required.");

            RuleFor(p => p.PermanentAddress.District).NotEmpty()
                .When(p => p.PermanentAddress != null)
                .WithMessage("Present address district is required.");

            RuleFor(p => p.PermanentAddress.CountryCode).NotEmpty()
                .When(p => p.PermanentAddress != null)
                .WithMessage("Present address country is required.");

            RuleFor(p => p.BusinessAddress.Street).NotEmpty()
                .When(p => p.BusinessAddress != null &&
                (!string.IsNullOrEmpty(p.BusinessAddress.District) || !string.IsNullOrEmpty(p.BusinessAddress.CountryCode)))
                .WithMessage("Business address street, district, and country must be filled togather.");

            RuleFor(p => p.BusinessAddress.District).NotEmpty()
                .When(p => p.BusinessAddress != null &&
                (!string.IsNullOrEmpty(p.BusinessAddress.Street) || !string.IsNullOrEmpty(p.BusinessAddress.CountryCode)))
                .WithMessage("Business address street, district, and country must be filled togather.");

            RuleFor(p => p.BusinessAddress.CountryCode).NotEmpty()
                .When(p => p.BusinessAddress != null &&
                (!string.IsNullOrEmpty(p.BusinessAddress.District) || !string.IsNullOrEmpty(p.BusinessAddress.Street)))
                .WithMessage("Business address street, district, and country must be filled togather.");

            RuleFor(p => p.CountryOfBirth)
                .Must(p => !string.IsNullOrEmpty(p))
                .WithMessage("Country of Birth Required");

            RuleFor(p => p.PlaceOfBirth)
                .Must(p => !string.IsNullOrEmpty(p))
                .WithMessage("Place of Birth Required");

            RuleFor(p => p.NationalIDNumber)
                .Must(p => !string.IsNullOrEmpty(p) && nidRegex.IsMatch(p))
                .When(p => string.IsNullOrEmpty(p.IDNumber))
                .WithMessage("NID must be 17 digits");

            RuleFor(p => p.IDNumber)
                .NotEmpty()
                .When(p => string.IsNullOrEmpty(p.SmartIDNumber))
                .WithMessage("NID or ID must be given");

            RuleFor(p => p.IDIssueDate)
                .NotEmpty()
                .When(p => !string.IsNullOrEmpty(p.IDNumber))
                .WithMessage("ID Issue Date Required");

            RuleFor(p => p.IDType)
                .NotEmpty()
                .When(p => !string.IsNullOrEmpty(p.IDNumber))
                .WithMessage("ID Type Required");

            RuleFor(p => p.IDIssueCountryCode)
                .NotEmpty()
                .When(p => !string.IsNullOrEmpty(p.IDNumber))
                .WithMessage("ID Issue Country Required");

            RuleFor(p => p.SmartIDNumber)
                .Must(p => !string.IsNullOrEmpty(p))
                .When(p => p.CountryOfBirth == "BD")
                .WithMessage("Smart ID required for BD person");

            RuleFor(p => p.SmartIDNumber)
                .Must(p => !string.IsNullOrEmpty(p) && sidRegex.IsMatch(p))
                .When(p => string.IsNullOrEmpty(p.IDNumber))
                .WithMessage("Smart ID must be 10 digits");
        }
    }
}
