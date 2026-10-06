using CIBnext.DAO;

using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class InstalmentContractBaseValidator<T> : ContractValidator<T> where T : InstalmentContractBase
    {
        public InstalmentContractBaseValidator()
        {
            //Overdue Amount
            RuleFor(c => c.OverdueAmount).NotEmpty().When(c => c.NumberOfOverdueInstallment != 0)
                .WithMessage("Overdue Amount required when Number Of Overdur Instalment Given");

            //Number of Overdue Instalment
            RuleFor(c => c.NumberOfOverdueInstallment).NotEmpty().When(c => c.OverdueAmount != 0)
                .WithMessage("No of Overdue Instalment required when Overdue Amount Given");

            //Instalment Amount
            RuleFor(c => c.InstalmentAmount).NotEmpty()
                .WithMessage("Installment Amount is required");

            When(c => new[] { "TM", "TA" }.Any(p => p == c.ContractPhase), () =>
            {
                RuleFor(c => c.RemainingAmount).Empty()
                    .WithMessage("Remaining amount should be zero for terminating contract");
                RuleFor(c => c.NumberOfOverdueInstallment).Empty()
                    .WithMessage("No of Overdue Instalment should be zero for terminatting contract");
                RuleFor(c => c.OverdueAmount).Empty()
                    .WithMessage("Overdue amount should be zero for terminatting contract");
                RuleFor(c => c.ExpirationDateOfNextInstallment).Empty()
                    .WithMessage("Expiration Date of Next Instalment should be empty for terminatting contract");
                RuleFor(c => c.CumulativeRecovery).Empty()
                    .WithMessage("Cumulative Recovery should be zero for terminatting contract");
                RuleFor(c => c.DateOfLastPayment).Empty()
                    .WithMessage("Date of Last Payment should be empty for terminatting contract");
            });
        }
    }
}