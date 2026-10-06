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
    public class NonInstalmentContractValidationTest
    {
        private NonInstalmentContractValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new NonInstalmentContractValidator();
        }

        [Test]
        public void NonCardValidation_Check()
        {
            var contract = new NonInstalmentContract();
            contract.EnterpriseType = "VAL";

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.SME);
        }
    }
}
