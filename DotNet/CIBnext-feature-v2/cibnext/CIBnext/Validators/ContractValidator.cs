using CIBnext.DAO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.Validators
{
    public class ContractValidator<T> : AbstractValidator<T> where T : Contract
    {
        public ContractValidator()
        {
            RuleFor(c => c.FICode).NotEmpty().WithMessage("FI Code is required");
            RuleFor(c => c.FIContractCode).NotEmpty().WithMessage("FI Contract Code is required");
            RuleFor(c => c.UbsAccountNo).NotEmpty().Length(16).WithMessage("UBS Account Number incorrect");
            RuleFor(c => c.ContractType).NotEmpty().WithMessage("Contract Type is required");
            RuleFor(c => c.ContractPhase).NotEmpty().WithMessage("Contract Phase is required");
            RuleFor(c => c.ContractStatus).NotEmpty().WithMessage("Contract Status is required");
            RuleFor(c => c.CurrencyCode).NotEmpty().WithMessage("Currency Code is required");
            RuleFor(c => c.DefaulterStatus).NotEmpty().WithMessage("Defaulter Status is required");
            RuleFor(c => c.FISubjectCode).NotEmpty().Length(16).WithMessage("Subject code incorrect");

            //Third Party Guarantee
            RuleFor(c => c.ThirdPartyGuaranteeType).NotEmpty().When(c => c.AmountGuaranteedByThirdParty > 0)
                .WithMessage("3rd party guarantee type required when amount given");
            RuleFor(c => c.AmountGuaranteedByThirdParty).NotEqual(0).When(c => !string.IsNullOrEmpty(c.ThirdPartyGuaranteeType))
                .WithMessage("3rd party guarantee amount required when type given");

            //Security Type
            RuleFor(c => c.SecurityType).NotEmpty().When(c => c.AmountGuaranteedBySecurityType > 0)
                .WithMessage("Security type required when amount given");
            RuleFor(c => c.AmountGuaranteedBySecurityType).NotEqual(0).When(c => !string.IsNullOrEmpty(c.SecurityType))
                .WithMessage("Security amount required when type given");

            //Default Status
            RuleFor(c => c.DefaulterStatus).Must(BeYorW).When(c => new[] { "D", "B", "W" }.Any(s => s == c.ContractStatus))
                .WithMessage("Defaulter status should be 'Y'");
            RuleFor(c => c.DefaulterStatus).Equal("N").When(c => new[] { "U", "M" }.Any(s => s == c.ContractStatus))
                .WithMessage("Defaulter status should be 'N'");

            //Date of Last Payment
            RuleFor(c => c.DateOfLastPayment).GreaterThan(c => c.StartingDate)
                .WithMessage("Date of Last Payment should be greater than Starting Date");

            //Date of Classsification
            RuleFor(c => c.DateOfClassification).Null().When(c => new[] { "U", "M" }.Any(s => s == c.ContractStatus))
                .WithMessage("Date of Classification should be empty for unclassified contracts");
            RuleFor(c => c.DateOfClassification).NotNull().When(c => new[] { "S", "D", "B", "W" }.Any(s => s == c.ContractStatus))
                .WithMessage("Date of Classification should not be empty for classified contracts");

            //Date of Law Suit
            RuleFor(c => c.DateOfLawSuit).GreaterThan(c => c.StartingDate)
                .WithMessage("Date of Law Suit should be greater than Starting Date");

            //Actual End Date
            RuleFor(c => c.ActualEndDate).Empty().When(c => c.ContractPhase == "LV")
                .WithMessage("Living Contract Should not Have Actual End Date");

            //Request Date
            RuleFor(c => c.RequestDate).Equal(c => c.StartingDate)
                .WithMessage("Request Date should be same as Starting Date");

            //Contract Phase
            When(c => new[] { "TM", "TA" }.Any(p => p == c.ContractPhase), () => {
                RuleFor(c => c.TotalOutstandingAmount).Empty()
                    .WithMessage("Total Outstanding Amount should be zero for terminating contract");
                RuleFor(c => c.ActualEndDate).NotEmpty()
                    .WithMessage("Actual End Date required for terminating contract");
                RuleFor(c => c.ActualEndDate).Equal(c => c.ReporingPeriod)
                    .WithMessage("Actual End Date Should be same as Reporting Period");
                RuleFor(c => c.DefaulterStatus).Equal("N")
                    .WithMessage("Defaulter Status should be No for terminating contract");
                RuleFor(c => c.DueForRecovery).Empty()
                    .WithMessage("Due for recoverty should be zero for terminating contract");
                RuleFor(c => c.DateOfLawSuit).Empty()
                    .WithMessage("No Date of Lawsuit for terminating contract");
                RuleFor(c => c.DateOfClassification).Empty()
                    .WithMessage("No Classification Date for terminating contract");
            });
        }

        private bool BeYorW(string defaultStatus)
        {
            return defaultStatus == "Y" || defaultStatus == "W";
        }
    }
}