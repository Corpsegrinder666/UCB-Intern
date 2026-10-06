using CIBnext.DAO;
using CIBnext.Validators;

using FluentValidation;
using FluentValidation.TestHelper;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIBnext.NTest
{
    public class InstitutionValidationTest
    {
        private InstitutionValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new InstitutionValidator();
        }

        [Test]
        public void BranchCode_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.BranchCode);
        }

        [Test]
        public void CIF_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.CifNo);
        }

        [Test]
        public void LegalForm_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.LegalForm);
        }

        [Test]
        public void RJSCNO_Required_For_Pvt_Pub()
        {
            var inst = new Institution();
            inst.LegalForm = 3;

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationNumberRJSC);

            inst.LegalForm = 4;

            result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationNumberRJSC);

            inst.LegalForm = 1;

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.RegistrationNumberRJSC);
        }

        [Test]
        public void RJSCDate_Required_For_Pvt_Pub()
        {
            var inst = new Institution();
            inst.LegalForm = 3;

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationDateRJSC);

            inst.LegalForm = 4;

            result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationDateRJSC);

            inst.LegalForm = 1;

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.RegistrationDateRJSC);
        }

        [Test]
        public void RJSCNo_Required_When_RJSCDate_Given()
        {
            var inst = new Institution();
            inst.RegistrationDateRJSC = DateTime.Now;

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationNumberRJSC);

            inst.RegistrationNumberRJSC = "something";

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.RegistrationNumberRJSC);
        }

        [Test]
        public void RJSCDate_Required_When_RJSCNo_Given()
        {
            var inst = new Institution();
            inst.RegistrationNumberRJSC = "something";

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.RegistrationDateRJSC);

            inst.RegistrationDateRJSC = DateTime.Now;

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.RegistrationDateRJSC);
        }

        [Test]
        public void TradeName_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.TradeName);

            inst.TradeName = "Name";

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.TradeName);
        }

        [Test]
        public void PhoneNumber_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.PhoneNumber);
        }

        [Test]
        public void PhoneNumber_Only_Digits()
        {
            var inst = new Institution();

            inst.PhoneNumber = "asldfs";

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.PhoneNumber);

            inst.PhoneNumber = "018234";

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.PhoneNumber);
        }

        [Test]
        public void SectorType_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.SectorType);

            inst.SectorType = 1;

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.SectorType);
        }

        [Test]
        public void SectorCode_Required()
        {
            var inst = new Institution();

            var result = validator.TestValidate(inst);
            result.ShouldHaveValidationErrorFor(i => i.SectorCode);

            inst.SectorCode = 1;

            result = validator.TestValidate(inst);
            result.ShouldNotHaveValidationErrorFor(i => i.SectorCode);
        }

        [Test]
        public void BusinessAddress_Required()
        {
            var institution = new Institution
            {
                BusinessAddress = null
            };
            var result = validator.TestValidate(institution);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress);
        }

        [Test]
        public void BusinessAddressStreetCountryDistric_Required()
        {
            var institution = new Institution
            {
                BusinessAddress = new Address { }
            };
            var result = validator.TestValidate(institution);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.District);
        }

        [Test]
        public void BusinessAddressStreet()
        {
            var institution = new Institution
            {
                BusinessAddress = new Address { Street = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.BusinessAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.District);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.CountryCode);
        }

        [Test]
        public void BusinessAddressDistrict()
        {
            var institution = new Institution
            {
                BusinessAddress = new Address { District = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.BusinessAddress.District);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.CountryCode);
        }

        [Test]
        public void BusinessAddressCountryCode()
        {
            var institution = new Institution
            {
                BusinessAddress = new Address { CountryCode = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.BusinessAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.BusinessAddress.District);
        }

        [Test]
        public void FactoryAddress_Required()
        {
            var institution = new Institution
            {
                FactoryAddress = null
            };
            var result = validator.TestValidate(institution);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress);
        }

        [Test]
        public void FactoryAddressStreetCountryDistrict_Required()
        {
            var institution = new Institution
            {
                FactoryAddress = new Address { }
            };
            var result = validator.TestValidate(institution);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.District);
        }

        [Test]
        public void FactoryAddressStreet()
        {
            var institution = new Institution
            {
                FactoryAddress = new Address { Street = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.FactoryAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.District);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.CountryCode);
        }

        [Test]
        public void FactoryAddressDistrict()
        {
            var institution = new Institution
            {
                FactoryAddress = new Address { District = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.FactoryAddress.District);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.CountryCode);
        }

        [Test]
        public void FactoryAddressCountryCode()
        {
            var institution = new Institution
            {
                FactoryAddress = new Address { CountryCode = "test" }
            };
            var result = validator.TestValidate(institution);
            result.ShouldNotHaveValidationErrorFor(i => i.FactoryAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.Street);
            result.ShouldHaveValidationErrorFor(i => i.FactoryAddress.District);
        }
    }
}
