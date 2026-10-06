using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class ContractLinkValidator : AbstractValidator<ContractLink>
    {
        public ContractLinkValidator()
        {
            RuleFor(l => l.FISecondaryCode).NotEqual(l => l.FIPrimaryCode)
                .WithMessage("Linked subject and Applicant are the same subject");

            RuleFor(l => l.FIContractCode).NotEmpty()
                .WithMessage("Contract Code required");

            RuleFor(l => l.FIPrimaryCode).NotEmpty()
                .WithMessage("Contract Code not found");

            RuleFor(l => l.FISecondaryCode).Length(16)
                .WithMessage("Invalid Secondary Code");

            RuleFor(l => l.FISecondaryName).NotEqual("Not found")
                .WithMessage("Invalid Secondary Code");

            RuleFor(l => l.BranchCode).NotEmpty()
                .WithMessage("Branch Code is required");
        }
    }
}





