using NUnit.Framework;
using FluentValidation;
using FluentValidation.TestHelper;
using CIBnext.Validators;
using CIBnext.DAO;
using System;

namespace CIBnext.NTest
{
    public class PersonalDataValidationTest
    {
        private PersonalDataValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new PersonalDataValidator();
        }

        [Test]
        public void SectorCodeAndSectorType_BothEmpty()
        {
            var personal = new PersonalData
            {
                SectorType = 0,
                SectorCode = 0
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.SectorCode);
            result.ShouldNotHaveValidationErrorFor(p => p.SectorType);
        }

        [Test]
        public void SectorCodeAndSectorType_CodeEmpty()
        {
            var personal = new PersonalData
            {
                SectorType = 1,
                SectorCode = 0
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.SectorCode);
        }

        [Test]
        public void SectorCodeAndSectorType_TypeEmpty()
        {
            var personal = new PersonalData
            {
                SectorType = 0,
                SectorCode = 1
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.SectorType);
        }

        [Test]
        public void PresentAddressNull()
        {
            var personal = new PersonalData
            {
                PresentAddress = null
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.Street);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.CountryCode);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.District);
        }

        [Test]
        public void PresentAddressStreetCountryDistrict_Required()
        {
            var personal = new PersonalData
            {
                PresentAddress = new Address { }
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.Street);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.District);
        }

        [Test]
        public void PresentAddressStreet()
        {
            var personal = new PersonalData
            {
                PresentAddress = new Address { Street = "test" }
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.Street);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.District);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.CountryCode);
        }

        [Test]
        public void PresentAddressDistrict()
        {
            var personal = new PersonalData
            {
                PresentAddress = new Address { District = "test" }
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.District);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.Street);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.CountryCode);
        }

        [Test]
        public void PresentAddressCountryCode()
        {
            var personal = new PersonalData
            {
                PresentAddress = new Address { CountryCode = "test" }
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.PresentAddress.CountryCode);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.Street);
            result.ShouldHaveValidationErrorFor(p => p.PresentAddress.District);
        }

        [Test]
        public void NatianalIDNumberOrID()
        {
            var personal = new PersonalData
            {
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.NationalIDNumber);
            result.ShouldHaveValidationErrorFor(p => p.IDNumber);
        }

        [Test]
        public void SmartID_NotRequiredWhenOtherIDGiven()
        {
            var personal = new PersonalData
            {
                IDNumber = "test",
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.SmartIDNumber);
        }

        [Test]
        public void ID_NotRequiredWhenSmartIDGiven()
        {
            var personal = new PersonalData
            {
                SmartIDNumber = "1111111111"
            };
            var result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.IDNumber);
        }

        [Test]
        public void ID_CountryRequired()
        {
            var personal = new PersonalData
            {
                IDNumber = "test"
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.IDIssueCountryCode);
        }

        [Test]
        public void ID_TypeRequired()
        {
            var personal = new PersonalData
            {
                IDNumber = "test"
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.IDType);
        }

        [Test]
        public void ID_IssueDateRequired()
        {
            var personal = new PersonalData
            {
                IDNumber = "test"
            };
            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.IDIssueDate);
        }

        [Test]
        public void SmartID_Required()
        {
            var personal = new PersonalData
            {
            };

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.SmartIDNumber);
        }

        [Test]
        public void SmartID_Should_Be_10_Digit_Numeric()
        {
            var personal = new PersonalData
            {
                SmartIDNumber = "test"
            };

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.SmartIDNumber);

            personal.SmartIDNumber = "1010101010";
            result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.SmartIDNumber);
        }

        [Test]
        public void SmartID_Required_When_CountryBD()
        {
            var personal = new PersonalData
            {
                CountryOfBirth = "BD"
            };

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.SmartIDNumber);
        }


        [Test]
        public void CountryOfBirth_Required()
        {
            var personal = new PersonalData();

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.CountryOfBirth);
        }

        [Test]
        public void PlaceOfBirth_Required()
        {
            var personal = new PersonalData();

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.PlaceOfBirth);
        }

        [Test]
        public void Gender_Required()
        {
            var personal = new PersonalData();

            var result = validator.TestValidate(personal);
            result.ShouldHaveValidationErrorFor(p => p.Gender);

            personal.Gender = "M";
            result = validator.TestValidate(personal);
            result.ShouldNotHaveValidationErrorFor(p => p.Gender);
        }
    }
}