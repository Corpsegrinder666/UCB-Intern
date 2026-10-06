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
    public class OwnerLinkValidationTest
    {
        private OwnerLinkValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new OwnerLinkValidator();
        }

        [Test]
        public void InstituteAndOwner_NotSame()
        {
            var link = new OwnerLink
            {
                InstitutionFiSubjectCode = "XXXXXXXXXXXXXXXX",
                OwnerFiSubjectCode = "XXXXXXXXXXXXXXXX",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.OwnerFiSubjectCode);
        }

        [Test]
        public void Owner_Length16()
        {
            var link = new OwnerLink
            {
                OwnerFiSubjectCode = "XXXXXXXXXXXXXXX",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.OwnerFiSubjectCode);
        }

        [Test]
        public void Institution_Length16()
        {
            var link = new OwnerLink
            {
                InstitutionFiSubjectCode = "XXXXXXXXXXXXXXX",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.InstitutionFiSubjectCode);
        }

        [Test]
        public void Owner_NotFound()
        {
            var link = new OwnerLink
            {
                OwnerFiSubjectName = "Not found",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.OwnerFiSubjectName);
        }

        [Test]
        public void Institution_NotFound()
        {
            var link = new OwnerLink
            {
                InstitutionFiSubjectName = "Not found",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.InstitutionFiSubjectName);
        }

        [Test]
        public void WrongRole()
        {
            var link = new OwnerLink
            {
                InstitutionFiSubjectCode = "IIIIIIIIIIIIIIII",
                Role = "11",
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.Role);

            link = new OwnerLink
            {
                InstitutionFiSubjectCode = "CIIIIIIIIIIIIIII",
                Role = "10",
            };
            result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.InstitutionFiSubjectCode);
        }

        [Test]
        public void BranchCodeRequired()
        {
            var link = new OwnerLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.CibBranchCode);
        }

        [Test]
        public void RoleRequired()
        {
            var link = new OwnerLink
            {
            };
            var result = validator.TestValidate(link);
            result.ShouldHaveValidationErrorFor(p => p.Role);
        }

        [Test]
        public void PositiveCase()
        {

            var link = new OwnerLink
            {
                CibBranchCode = "XXXX",
                InstitutionFiSubjectCode = "IIIIIIIIIIIIIIII",
                InstitutionFiSubjectName = "IIIIIIIIIIIIIIII",
                OwnerFiSubjectCode = "PIIIIIIIIIIIIIII",
                OwnerFiSubjectName = "PIIIIIIIIIIIIIII",
                Role = "10",
            };
            var result = validator.TestValidate(link);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
