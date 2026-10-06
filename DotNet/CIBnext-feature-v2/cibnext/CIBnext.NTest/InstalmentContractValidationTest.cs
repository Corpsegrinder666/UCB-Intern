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
    public class InstalmentContractValidationTest
    {
        private InstalmentContractValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new InstalmentContractValidator();
        }

        [Test]
        public void ExpiryDateOfNextInstallment_GreaterThan_StartDate()
        {
            var contract = new InstalmentContract();
            contract.StartingDate = DateTime.Now;
            contract.ExpirationDateOfNextInstallment = contract.StartingDate.AddDays(-1);

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);

            contract.StartingDate = DateTime.Now;
            contract.ExpirationDateOfNextInstallment = contract.StartingDate.AddDays(1);

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);

            //is optional
            contract.ExpirationDateOfNextInstallment = null;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);
        }

        [Test]
        public void ExpiryDateOfNextInstallment_GreaterThan_ReportingPeriod()
        {
            var contract = new InstalmentContract();
            contract.ReporingPeriod = DateTime.Now;
            contract.ExpirationDateOfNextInstallment = contract.ReporingPeriod.AddDays(-1);

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);

            contract.ExpirationDateOfNextInstallment = contract.ReporingPeriod.AddDays(1);

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);
        }

        [Test]
        public void RemainingAmount_NoOfRemainingInstalment()
        {
            var contract = new InstalmentContract();
            contract.RemainingAmount = 0;
            contract.NumberOfRemainingInstallments = 1;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.RemainingAmount);

            contract.RemainingAmount = 1;
            contract.NumberOfRemainingInstallments = 0;

            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.NumberOfRemainingInstallments);
        }

        [Test]
        public void NonCardValidation_Check()
        {
            var contract = new InstalmentContract();
            contract.EnterpriseType = "VAL";

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.SME);
        }

        [Test]
        public void InstalmentContractBase_Check()
        {
            var contract = new InstalmentContract();
            contract.NumberOfOverdueInstallment = 1;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.OverdueAmount);
        }

        [Test]
        public void RemainingNOverdueInstalment_LessThan_TotalInstalment()
        {
            var contract = new InstalmentContract();
            contract.TotalNumberOfInstallments = 2;
            contract.RemainingAmount = 10;
            contract.NumberOfRemainingInstallments = 10;
            contract.OverdueAmount = 8;
            contract.NumberOfOverdueInstallment = 8;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.TotalNumberOfInstallments);

            contract.TotalNumberOfInstallments = 20;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.TotalNumberOfInstallments);

            contract.NumberOfOverdueInstallment = 10;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.TotalNumberOfInstallments);
        }

        [Test]
        public void Terminate_Test()
        {
            var contract = new InstalmentContract();
            contract.ContractPhase = "TM";
            contract.NumberOfRemainingInstallments = 1;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.NumberOfRemainingInstallments);
        }

        [Test]
        public void Terminate_Test_Reverse()
        {
            var contract = new InstalmentContract();
            contract.ContractPhase = "LV";
            contract.NumberOfRemainingInstallments = 1;

            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.NumberOfRemainingInstallments);
        }
    }
}
