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
    public class ContractLinkValidationTest
    {
        private ContractLinkValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new ContractLinkValidator();
        }

        [Test]
        public void PrimaryAndSecondary_NotSame()
        {
            var link = new ContractLink
            {
                FIPrimaryCode = "XXXXXXXXXXXXXXXX",
                FISecondaryCode = "XXXXXXXXXXXXXXXX",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.FISecondaryCode);
        }

        [Test]
        public void PrimaryCode_Required()
        {
            var link = new ContractLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.FIPrimaryCode);
        }

        [Test]
        public void ContractCode_Required()
        {
            var link = new ContractLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.FIContractCode);
        }

        [Test]
        public void SecondaryCode_Required()
        {
            var link = new ContractLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.FISecondaryCode);
        }

        [Test]
        public void SecondaryCode_Length16()
        {
            var link = new ContractLink
            {
                FISecondaryCode = "XXXX"
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.FISecondaryCode);
            Assert.AreEqual(
            result.Errors.Where(e => e.PropertyName.Equals(nameof(link.FISecondaryCode))).FirstOrDefault().ErrorMessage,
            "Invalid Secondary Code");
        }

        [Test]
        public void SecondaryName_NotFound()
        {
            var link = new ContractLink
            {
                FISecondaryName = "Not found"
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.FISecondaryName);
        }

        [Test]
        public void BranchCode_Required()
        {
            var link = new ContractLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(l => l.BranchCode);
        }

        [Test]
        public void PositiveCase()
        {

            var link = new ContractLink
            {
                FIContractCode = "IIIIIIIIIIIIIIII",
                FIPrimaryCode = "IIIIIIIIIIIIIIII",
                FISecondaryCode = "PIIIIIIIIIIIIIII",
                FISecondaryName = "PIIIIIIIIIIIIIII",
                BranchCode = "0000",
            };
            var result = validator.TestValidate(link);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
