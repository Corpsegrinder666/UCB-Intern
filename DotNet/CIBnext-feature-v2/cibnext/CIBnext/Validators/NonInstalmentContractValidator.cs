using CIBnext.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;

namespace CIBnext.Validators
{
    public class NonInstalmentContractValidator : ContractValidator<NonInstalmentContract>
    {
        public NonInstalmentContractValidator()
        {
            RuleFor(c => c.BranchCode).NotEmpty().WithMessage("Branch not selected");
            RuleFor(c => c.SanctionLimit).GreaterThan(0).WithMessage("Sanction Limit is required");
            RuleFor(c => c.TotalOutstandingAmount).GreaterThanOrEqualTo(0).WithMessage("Invalid Outstanding Amount");

            RuleFor(c => c as INonCard).SetValidator(new NonCardValidator());
        }
    }
}
