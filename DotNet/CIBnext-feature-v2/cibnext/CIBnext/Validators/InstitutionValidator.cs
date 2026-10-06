using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace CIBnext.Validators
{
    public class InstitutionValidator : AbstractValidator<Institution>
    {
        public InstitutionValidator()
        {
            var phRgx = new Regex(@"\d+");
            RuleFor(i => i.BranchCode).NotEmpty().WithMessage("Branch code is required");
            RuleFor(i => i.CifNo).NotEmpty().WithMessage("CIF is required");
            RuleFor(i => i.LegalForm).NotEmpty().WithMessage("Legal Form is required");

            RuleFor(i => i.RegistrationNumberRJSC)
                .Must(i => !string.IsNullOrEmpty(i))
                .When(i => i.LegalForm == 3 || i.LegalForm == 4)
                .WithMessage("RJSC No required for Public and Private company");

            RuleFor(i => i.RegistrationDateRJSC)
                .Must(i => i != default(DateTime))
                .When(i => i.LegalForm == 3 || i.LegalForm == 4)
                .WithMessage("RJSC No required for Public and Private company");

            RuleFor(i => i.RegistrationNumberRJSC)
                .NotEmpty()
                .When(i => i.RegistrationDateRJSC != default(DateTime))
                .WithMessage("RJSC No required when Date Given");

            RuleFor(i => i.RegistrationDateRJSC)
                .NotEmpty()
                .When(i => !string.IsNullOrEmpty(i.RegistrationNumberRJSC))
                .WithMessage("RJSC Date required when number Given");

            RuleFor(i => i.TradeName).NotNull().NotEmpty()
                .WithMessage("Trade Name is Required");

            RuleFor(i => i.PhoneNumber).NotNull().NotEmpty()
                .WithMessage("Phone Number required");

            RuleFor(i => i.PhoneNumber).Must(num => phRgx.IsMatch(num ?? ""))
                .WithMessage("Phone number must only contain digits");

            RuleFor(i => i.SectorType).NotNull().NotEqual(0)
                .WithMessage("Sector Type is required");

            RuleFor(i => i.SectorCode).NotNull().NotEqual(0)
                .WithMessage("Sector Code is required");

            RuleFor(i => i.BusinessAddress).NotNull()
                .WithMessage("Business address is required");

            RuleFor(i => i.BusinessAddress.Street).NotEmpty()
                .When(p => p.BusinessAddress != null)
                .WithMessage("Business address street is required.");

            RuleFor(i => i.BusinessAddress.District).NotEmpty()
                .When(p => p.BusinessAddress != null)
                .WithMessage("Business address district is required.");

            RuleFor(i => i.BusinessAddress.CountryCode).NotEmpty()
                .When(p => p.BusinessAddress != null)
                .WithMessage("Business address country is required.");

            RuleFor(i => i.FactoryAddress).NotNull()
                .WithMessage("Factory address is required");

            RuleFor(i => i.FactoryAddress.Street).NotEmpty()
                .When(p => p.FactoryAddress != null)
                .WithMessage("Factory address street is required");

            RuleFor(i => i.FactoryAddress.District).NotEmpty()
                .When(p => p.FactoryAddress != null)
                .WithMessage("Factory address district is required");

            RuleFor(i => i.FactoryAddress.CountryCode).NotEmpty()
                .When(p => p.FactoryAddress != null)
                .WithMessage("Factory address country is required");
        }
    }
}
