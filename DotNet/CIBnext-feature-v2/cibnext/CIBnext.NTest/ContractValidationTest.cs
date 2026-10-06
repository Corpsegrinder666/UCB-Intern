using CIBnext.DAO;
using CIBnext.Validators;

using FluentValidation.TestHelper;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIBnext.NTest
{
    public class ContractValidationTest
    {
        private ContractValidator<Contract> validator;

        [SetUp]
        public void Setup()
        {
            this.validator = new ContractValidator<Contract>();
        }

        [Test]
        public void SubjectCode_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.FISubjectCode);
        }

        [Test]
        public void SubjectCode_Length16()
        {
            var contract = new Contract
            { 
                FISubjectCode = "NOT16"
            };
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.FISubjectCode);
        }

        [Test]
        public void CBSAc_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.UbsAccountNo);
        }

        [Test]
        public void CBSAc_Length16()
        {
            var contract = new Contract
            { 
                UbsAccountNo = "NOT16"
            };
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.UbsAccountNo);
        }

        [Test]
        public void ContractType_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ContractType);
        }

        [Test]
        public void ContractPhase_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ContractPhase);
        }

        [Test]
        public void ContractStatus_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ContractStatus);
        }

        [Test]
        public void CurrencyCode_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.CurrencyCode);
        }

        [Test]
        public void DefaulterStatus_Required()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.CurrencyCode);
        }

        [Test]
        public void ThirtPartyGuarantee_BothOrNone()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ThirdPartyGuaranteeType);
            result.ShouldNotHaveValidationErrorFor(c => c.AmountGuaranteedByThirdParty);

            contract.ThirdPartyGuaranteeType = "DM";
            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ThirdPartyGuaranteeType);
            result.ShouldHaveValidationErrorFor(c => c.AmountGuaranteedByThirdParty);

            contract.AmountGuaranteedByThirdParty = 1;
            contract.ThirdPartyGuaranteeType = null;
            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ThirdPartyGuaranteeType);
            result.ShouldNotHaveValidationErrorFor(c => c.AmountGuaranteedByThirdParty);
        }

        [Test]
        public void Security_BothOrNone()
        {
            var contract = new Contract();
            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.SecurityType);
            result.ShouldNotHaveValidationErrorFor(c => c.AmountGuaranteedBySecurityType);

            contract.SecurityType = "DM";
            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.SecurityType);
            result.ShouldHaveValidationErrorFor(c => c.AmountGuaranteedBySecurityType);

            contract.AmountGuaranteedBySecurityType = 1;
            contract.SecurityType = null;
            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.SecurityType);
            result.ShouldNotHaveValidationErrorFor(c => c.AmountGuaranteedBySecurityType);
        }

        [Test]
        public void ContractStatus_DefaulterStatus()
        {
            var contract = new Contract();
            contract.ContractStatus = "S";
            contract.DefaulterStatus = "N";
            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DefaulterStatus);

            contract.ContractStatus = "S";
            contract.DefaulterStatus = "Y";
            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DefaulterStatus);

            contract.ContractStatus = "S";
            contract.DefaulterStatus = "W";
            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DefaulterStatus);

            contract.ContractStatus = "U";
            contract.DefaulterStatus = "Y";
            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DefaulterStatus);

            contract.ContractStatus = "U";
            contract.DefaulterStatus = "W";
            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DefaulterStatus);

            contract.ContractStatus = "U";
            contract.DefaulterStatus = "N";
            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DefaulterStatus);
        }

        [Test]
        public void ContractStatus_DateOfClassification()
        {
            var contract = new Contract();
            contract.ContractStatus = "U";
            contract.DateOfClassification = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DateOfClassification);

            contract.ContractStatus = "S";
            contract.DateOfClassification = null;

            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DateOfClassification);

            contract.ContractStatus = "U";
            contract.DateOfClassification = null;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfClassification);

            contract.ContractStatus = "S";
            contract.DateOfClassification = DateTime.Now;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfClassification);
        }

        [Test]
        public void DateOfLastPayment_GreaterThan_StartDate()
        {
            var contract = new Contract();
            contract.StartingDate = DateTime.Now.Date;
            contract.DateOfLastPayment = contract.StartingDate.AddDays(-1);

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DateOfLastPayment);

            contract.DateOfLastPayment = contract.StartingDate.AddDays(1);

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLastPayment);

            //is optional
            contract.DateOfLastPayment = null;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLastPayment);
        }

        [Test]
        public void DateOfLawSuit_GreaterThan_StartDate()
        {
            var contract = new Contract();
            contract.StartingDate = DateTime.Now;
            contract.DateOfLawSuit = contract.StartingDate.AddDays(-1);

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.DateOfLawSuit);

            contract.StartingDate = DateTime.Now;
            contract.DateOfLawSuit = contract.StartingDate.AddDays(1);

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLawSuit);

            //is optional
            contract.DateOfLawSuit = null;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLawSuit);
        }

        [Test]
        public void ActualEndDate_EmptyForLiving()
        {
            var contract = new Contract();
            contract.ContractPhase = "LV";
            contract.ActualEndDate = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ActualEndDate);
        }

        [Test]
        public void ActualEndDate_SameAs_ReportingPeriod()
        {
            var contract = new Contract();
            contract.ActualEndDate = DateTime.Now.Date;
            contract.ReporingPeriod = DateTime.Now;
            contract.ContractPhase = "TM";

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ActualEndDate);

            contract.ActualEndDate = DateTime.Now.Date;
            contract.ReporingPeriod = contract.ActualEndDate;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ActualEndDate);

            contract.ActualEndDate = default;
            contract.ContractPhase = "LV";

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ActualEndDate);
        }

        [Test]
        public void StartingDate_SameAs_RequestDate()
        {
            var contract = new Contract();
            contract.StartingDate = DateTime.Now.Date;
            contract.RequestDate = DateTime.Now.AddDays(1);

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.RequestDate);

            contract.StartingDate = DateTime.Now.Date;
            contract.RequestDate = contract.StartingDate;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.RequestDate);
        }

        [Test]
        public void Termination_Test()
        {
            var contract = new Contract();
            contract.ContractPhase = "TM";
            contract.TotalOutstandingAmount = 1;
            contract.DefaulterStatus = "Y";
            contract.DueForRecovery = 1;
            contract.DateOfLawSuit = DateTime.Now;
            contract.DateOfClassification = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.TotalOutstandingAmount);
            result.ShouldHaveValidationErrorFor(c => c.ActualEndDate);
            result.ShouldHaveValidationErrorFor(c => c.DefaulterStatus);
            result.ShouldHaveValidationErrorFor(c => c.DueForRecovery);
            result.ShouldHaveValidationErrorFor(c => c.DateOfLawSuit);
            result.ShouldHaveValidationErrorFor(c => c.DateOfClassification);
        }

        [Test]
        public void Termination_Test_Reverse()
        {
            var contract = new Contract();
            contract.ContractPhase = "LV";
            contract.TotalOutstandingAmount = 1;
            contract.DefaulterStatus = "Y";
            contract.DueForRecovery = 1;
            contract.DateOfLawSuit = DateTime.Now;
            contract.DateOfClassification = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.TotalOutstandingAmount);
            result.ShouldNotHaveValidationErrorFor(c => c.ActualEndDate);
            result.ShouldNotHaveValidationErrorFor(c => c.DefaulterStatus);
            result.ShouldNotHaveValidationErrorFor(c => c.DueForRecovery);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLawSuit);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfClassification);
        }
    }
}
