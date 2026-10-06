using CIBnext.DAO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class InstalmentContractValidator : InstalmentContractBaseValidator<InstalmentContract>
    {
        public InstalmentContractValidator()
        {
            RuleFor(c => c.BranchCode).NotEmpty().WithMessage("Branch Not Selected");
            RuleFor(c => c.SanctionLimit).GreaterThan(0).WithMessage("Sanction Limit is required");
            RuleFor(c => c.TotalDisbursedAmount).GreaterThan(0).WithMessage("Total Disbursed amount is required");
            RuleFor(c => c.PeriodicityOfPayment).NotEmpty().WithMessage("Periodicity of Payment is required");
            RuleFor(c => c.TotalNumberOfInstallments).GreaterThan(0).WithMessage("Total Number of Instalment is required");
            RuleFor(c => c.TotalOutstandingAmount).GreaterThanOrEqualTo(0).WithMessage("Invalid Outstanding Amount");

            //Expiry Date of Next Instalment
            RuleFor(c => c.ExpirationDateOfNextInstallment).GreaterThan(c => c.StartingDate)
                .WithMessage("Expiry Date of Next Instalment should be greater than Starting Date");
            RuleFor(c => c.ExpirationDateOfNextInstallment).GreaterThan(c => c.ReporingPeriod)
                .WithMessage("Expiry Date of Next Instalment should be greater than Reporting Period");

            //Remaining Amount
            RuleFor(c => c.RemainingAmount).NotEmpty().When(c => c.NumberOfRemainingInstallments != 0)
                .WithMessage("Remaining Amount required when No Of Remaining Instalment Given");

            //No of Remaining Amount
            RuleFor(c => c.NumberOfRemainingInstallments).NotEmpty().When(c => c.RemainingAmount != 0)
                .WithMessage("No of Remaining Instalment required when Remaining Amount Given");

            //Total No of Instalment
            RuleFor(c => c.TotalNumberOfInstallments).GreaterThanOrEqualTo(c => c.NumberOfRemainingInstallments + c.NumberOfOverdueInstallment)
                .WithMessage("Total No of Instalment should be greater than remaining and overdue instalment no.");

            When(c => new[] { "TM", "TA" }.Any(p => p == c.ContractPhase), () =>
            {
                RuleFor(c => c.NumberOfRemainingInstallments).Empty()
                    .WithMessage("No of Remaining Installment should be zero for terminatting contract");
            });

            RuleFor(c => c as INonCard).SetValidator(new NonCardValidator());
        }
    }
}
