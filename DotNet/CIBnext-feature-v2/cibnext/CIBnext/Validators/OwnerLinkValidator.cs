using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class OwnerLinkValidator : AbstractValidator<OwnerLink>
    {
        public OwnerLinkValidator()
        {
            RuleFor(l => l.OwnerFiSubjectCode).NotEqual(l => l.InstitutionFiSubjectCode)
                .WithMessage("Linked subject and Applicant are the same subject");
            RuleFor(l => l.OwnerFiSubjectCode).Length(16)
                .WithMessage("Incorrect Owner Subject Code");
            RuleFor(l => l.OwnerFiSubjectName).NotEqual("Not found")
                .WithMessage("Incorrect Owner Subject Code");
            RuleFor(l => l.InstitutionFiSubjectCode).Length(16)
                .WithMessage("Incorrect Institution Subject Code");
            RuleFor(l => l.InstitutionFiSubjectName).NotEqual("Not found")
                .WithMessage("Incorrect Institution Subject Code");
            RuleFor(l => l.InstitutionFiSubjectCode).Must(l => !string.IsNullOrEmpty(l) && l.StartsWith("I"))
                .When(l => l.Role == "10")
                .WithMessage("Wrong role for proprietorship");
            RuleFor(l => l.Role).Must(r => r == "10")
                .When(l => !string.IsNullOrEmpty(l.InstitutionFiSubjectCode) && l.InstitutionFiSubjectCode.StartsWith("I"))
                .WithMessage("Wrong role for proprietorship");
            RuleFor(l => l.Role).NotEmpty()
                .WithMessage("Must select valid role");
            RuleFor(l => l.CibBranchCode).NotEmpty()
                .WithMessage("Must select branch");
        }
    }
}


