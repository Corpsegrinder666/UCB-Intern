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
    public class NonCardValidationTest
    {
        private NonCardValidator validator;

        [SetUp]
        public void SetUp()
        {
            validator = new NonCardValidator();
        }

        [Test]
        public void SME_EnterpriseType()
        {
            INonCard contract = new NonInstalmentContract();
            contract.SME = null;
            contract.EnterpriseType = "VAL";

            var result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.SME);

            contract.SME = "N";
            contract.EnterpriseType = "VAL";

            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.SME);

            contract.SME = "N";
            contract.EnterpriseType = null;

            result = validator.TestValidate(contract);
            result.ShouldNotHaveValidationErrorFor(c => c.SME);

            contract.SME = "Y";
            contract.EnterpriseType = null;

            result = validator.TestValidate(contract);
            result.ShouldHaveValidationErrorFor(c => c.EnterpriseType);
        }
    }
}
