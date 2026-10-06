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
    public class InstalmentContractBaseValidationTest
    {
        private InstalmentContractBaseValidator<InstalmentContractBase> validator;

        [SetUp]
        public void SetUp()
        {
            validator = new InstalmentContractBaseValidator<InstalmentContractBase>();
        }

        [Test]
        public void OverdueAmount_NoOfOverdueInstalment()
        {
            var contract = new InstalmentContractBase();
            contract.OverdueAmount = 0;
            contract.NumberOfOverdueInstallment = 1;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.OverdueAmount);

            contract.OverdueAmount = 1;
            contract.NumberOfOverdueInstallment = 0;

            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.NumberOfOverdueInstallment);
        }

        [Test]
        public void InstalmentAmount_Required()
        {
            var contract = new InstalmentContractBase();
            contract.InstalmentAmount = 0;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.InstalmentAmount);
        }

        [Test]
        public void Termination_Test()
        {
            var contract = new InstalmentContractBase();
            contract.ContractPhase = "TM";
            contract.RemainingAmount = 1;
            contract.NumberOfOverdueInstallment = 1;
            contract.OverdueAmount = 1;
            contract.ExpirationDateOfNextInstallment = DateTime.Now;
            contract.CumulativeRecovery = 1;
            contract.DateOfLastPayment = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.RemainingAmount);
            result.ShouldHaveValidationErrorFor(c => c.NumberOfOverdueInstallment);
            result.ShouldHaveValidationErrorFor(c => c.OverdueAmount);
            result.ShouldHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);
            result.ShouldHaveValidationErrorFor(c => c.CumulativeRecovery);
            result.ShouldHaveValidationErrorFor(c => c.DateOfLastPayment);
        }

        [Test]
        public void Termination_Test_Reverse()
        {
            var contract = new InstalmentContractBase();
            contract.ContractPhase = "LV";
            contract.RemainingAmount = 1;
            contract.NumberOfOverdueInstallment = 1;
            contract.OverdueAmount = 1;
            contract.ExpirationDateOfNextInstallment = DateTime.Now;
            contract.CumulativeRecovery = 1;
            contract.DateOfLastPayment = DateTime.Now;

            var result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.RemainingAmount);
            result.ShouldNotHaveValidationErrorFor(c => c.NumberOfOverdueInstallment);
            result.ShouldNotHaveValidationErrorFor(c => c.OverdueAmount);
            result.ShouldNotHaveValidationErrorFor(c => c.ExpirationDateOfNextInstallment);
            result.ShouldNotHaveValidationErrorFor(c => c.CumulativeRecovery);
            result.ShouldNotHaveValidationErrorFor(c => c.DateOfLastPayment);
        }
    }
}
