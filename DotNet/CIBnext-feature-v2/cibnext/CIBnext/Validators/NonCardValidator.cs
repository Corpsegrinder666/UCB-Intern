using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class NonCardValidator : AbstractValidator<INonCard>
    {
        public NonCardValidator()
        {
            //SME
            RuleFor(c => c.SME).Equal("Y").When(c => !string.IsNullOrEmpty(c.EnterpriseType))
                .WithMessage("SME value required when Enterprise Type Given");

            //Enterprise Type
            RuleFor(c => c.EnterpriseType).NotEmpty().When(c => !string.IsNullOrEmpty(c.SME) && c.SME == "Y")
                .WithMessage("Enterprise Type required for SME");
        }
    }
}