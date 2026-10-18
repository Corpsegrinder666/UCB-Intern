using CIBnext.BLL;
using CIBnext.DAL;
using CIBnext.DAO;
using CIBnext.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CIBnext.Pages.Contract
{
    [Authorize(Roles = "3002,ADMIN")]
    public class AddNonInstallmentContractModel : PageModel
    {
        private readonly ListOfValuesBLL _lovBLL;
        private readonly MiscBLL _miscBll;
        private readonly IConfiguration _configuration;

        public AddNonInstallmentContractModel(
            ListOfValuesBLL lovBLL,
            MiscBLL miscBll,
            IConfiguration configuration)
        {
            _lovBLL = lovBLL;
            _miscBll = miscBll;
            _configuration = configuration;
        }

        // =========================
        // Contract and checkbox fields
        // =========================

        [BindProperty]
        public NonInstalmentContract Contract { get; set; } =
            new NonInstalmentContract();

        [BindProperty]
        public bool SubsidizedCreditFlag { get; set; }

        [BindProperty]
        public bool PreFinanceFlag { get; set; }

        [BindProperty]
        public bool QualitativeJudgementFlag { get; set; }

        [BindProperty]
        public bool SmeFlag { get; set; }

        // =========================
        // Messages
        // =========================

        public string ValidationMessage { get; set; }

        public string ValidationMessageType { get; set; } = "warning";

        public string ValidationMessageTitle { get; set; } = "Validation";

        public string BorrowerLookupMessage { get; set; }

        public string BorrowerLookupMessageType { get; set; } = "warning";

        public string BorrowerLookupMessageTitle { get; set; } =
            "Borrower Lookup";

        public bool IsEdit { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        // =========================
        // Dropdowns
        // =========================

        public List<SelectListItem> BranchCodeList { get; set; }

        public List<SelectListItem> ContractTypeList { get; set; }

        public List<SelectListItem> ContractPhaseList { get; set; }

        public List<SelectListItem> ContractStatusList { get; set; }

        public List<SelectListItem> CurrencyList { get; set; }

        public List<SelectListItem> ReorganizedCreditList { get; set; }

        public List<SelectListItem> ThirdPartyGuaranteeTypeList { get; set; }

        public List<SelectListItem> SecurityTypeList { get; set; }

        public List<SelectListItem> DefaulterStatusList { get; set; }

        public List<SelectListItem> NoOfDaysPaymentDelayList { get; set; }

        public List<SelectListItem> EconomicPurposeCodeList { get; set; }

        public List<SelectListItem> EnterpriseTypeList { get; set; }

        // =========================
        // DROPDOWNS
        // =========================

        private void LoadDropdowns()
        {
            BranchCodeList = _miscBll.GetBranches().Result
                .Select(x => new SelectListItem
                {
                    Value = x.CibBranchCode,
                    Text = x.BranchName
                })
                .ToList();

            ContractTypeList = _lovBLL
                .GetListOfValues(LovTypes.CONTRACT_TYPES_NONINST)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            ContractPhaseList = _lovBLL
                .GetListOfValues(LovTypes.CONTRACT_PHASES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            ContractStatusList = _lovBLL
                .GetListOfValues(LovTypes.CONTRACT_STATUS)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            CurrencyList = _lovBLL
                .GetListOfValues(LovTypes.CURRENCIES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            ReorganizedCreditList = _lovBLL
                .GetListOfValues(LovTypes.REORGANIZED_CREDIT)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            ThirdPartyGuaranteeTypeList = _lovBLL
                .GetListOfValues(LovTypes.THIRD_PARTY_GUARANTEE_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            SecurityTypeList = _lovBLL
                .GetListOfValues(LovTypes.SECURITY_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            NoOfDaysPaymentDelayList = _lovBLL
                .GetListOfValues(LovTypes.NO_OF_DAYS)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            EconomicPurposeCodeList = _lovBLL
                .GetListOfValues(LovTypes.ECONOMIC_PURPOSE_CODES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            EnterpriseTypeList = _lovBLL
                .GetListOfValues(LovTypes.ENTERPRISE_TYPES)
                .Select(x => new SelectListItem
                {
                    Value = x.Key,
                    Text = x.Value
                })
                .ToList();

            // Defaulter Status - LOV does not exist.
            DefaulterStatusList = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "Y",
                    Text = "Yes"
                },
                new SelectListItem
                {
                    Value = "N",
                    Text = "No"
                }
            };
        }

        // =========================
        // GET
        // =========================

        public void OnGet(string contractCode)
        {
            LoadDropdowns();

            if (!string.IsNullOrEmpty(contractCode))
            {
                var contractDAL = new ContractDAL(_configuration);

                var existing =
                    contractDAL.SelectNonInstalmentContract(contractCode);

                if (existing != null)
                {
                    Contract = existing;
                    IsEdit = true;

                    SubsidizedCreditFlag =
                        Contract.FlagSubsidizedCredit == "Y";

                    PreFinanceFlag =
                        Contract.FlagPreFinanceOfLoan == "Y";

                    QualitativeJudgementFlag =
                        Contract.QualitativeJudgement == "Y";

                    SmeFlag = Contract.SME == "Y";
                }
                else
                {
                    ValidationMessage =
                        "Contract not found for code: " + contractCode;

                    ValidationMessageType = "error";
                    ValidationMessageTitle = "Contract Not Found";
                }
            }

            if (!IsEdit)
            {
                Contract.MakeDate = DateTime.Now;
            }
        }

        // =========================
        // BORROWER LOOKUP
        // =========================

        public IActionResult OnPostLookupBorrower()
        {
            LoadDropdowns();

            IsEdit = !string.IsNullOrEmpty(Contract.FIContractCode);

            if (string.IsNullOrEmpty(Contract.FISubjectCode))
            {
                BorrowerLookupMessage =
                    "Please enter F.I. Subject Code first.";

                BorrowerLookupMessageType = "warning";
                BorrowerLookupMessageTitle = "Borrower Lookup";

                return Page();
            }

            var subjectBLL = new SubjectBLL(_configuration);

            var person = subjectBLL.GetPerson(Contract.FISubjectCode);

            var institution =
                subjectBLL.GetInstitution(Contract.FISubjectCode);

            string name = person?.Name ?? institution?.TradeName;

            if (string.IsNullOrEmpty(name))
            {
                BorrowerLookupMessage =
                    "No subject found for this F.I. Subject Code.";

                BorrowerLookupMessageType = "warning";
                BorrowerLookupMessageTitle = "Borrower Not Found";
            }
            else
            {
                Contract.BorrowerName = name;

                BorrowerLookupMessage =
                    "Borrower found: " + name;

                BorrowerLookupMessageType = "success";
                BorrowerLookupMessageTitle = "Borrower Found";
            }

            return Page();
        }

        // =========================
        // SAVE
        // =========================

        public IActionResult OnPost()
        {
            LoadDropdowns();

            IsEdit = !string.IsNullOrEmpty(Contract.FIContractCode);

            Contract.FlagSubsidizedCredit =
                SubsidizedCreditFlag ? "Y" : "N";

            Contract.FlagPreFinanceOfLoan =
                PreFinanceFlag ? "Y" : "N";

            Contract.QualitativeJudgement =
                QualitativeJudgementFlag ? "Y" : "N";

            Contract.SME = SmeFlag ? "Y" : "N";

            Contract.RecordType = "N";
            Contract.FICode = "046";
            Contract.MakeBy = User.Identity?.Name;
            Contract.MakeDate = DateTime.Now;
            Contract.RecordStatus = "C";

            ModelState.Clear();

            ValidationMessage = ValidateContract(Contract);

            if (!string.IsNullOrEmpty(ValidationMessage))
            {
                ValidationMessageType = "warning";
                ValidationMessageTitle = "Validation";

                return Page();
            }

            var contractBLL = new ContractBLL(_configuration);

            bool success = contractBLL.SaveContract(Contract);

            if (!success)
            {
                ValidationMessage =
                    "Save failed. Please check the values and try again.";

                ValidationMessageType = "error";
                ValidationMessageTitle = "Save Failed";

                return Page();
            }

            SuccessMessage = IsEdit
                ? "Non-installment contract updated successfully."
                : "Non-installment contract saved successfully.";

            return RedirectToPage("/Contract/NonInstallmentContract");
        }

        // =========================
        // VALIDATION
        // =========================

        private string ValidateContract(NonInstalmentContract contract)
        {
            var validator = new NonInstalmentContractValidator();

            var result = validator.Validate(contract);

            if (!result.IsValid)
            {
                return "- " + string.Join(
                    "<br/>- ",
                    result.Errors
                        .Select(error => error.ErrorMessage)
                        .Distinct());
            }

            return string.Empty;
        }
    }
}
