using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Contract
    {
        public DateTime ReporingPeriod { get; set; }
        public string RecordType { get; set; }
        public string FICode { get; set; }
        public string BranchCode { get; set; }
        public string FISubjectCode { get; set; }
        public string BorrowerName { get; set; }
        public string FIContractCode { get; set; }
        public string UbsAccountNo { get; set; }
        public string ContractType { get; set; }
        public string ContractPhase { get; set; }
        public string ContractStatus { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string DefaulterStatus { get; set; }
        public DateTime? DateOfLastPayment { get; set; }
        public string FlagSubsidizedCredit { get; set; }
        public string FlagPreFinanceOfLoan { get; set; }
        public string CodeReorganizedCredit { get; set; }
        public string ThirdPartyGuaranteeType { get; set; }
        public string SecurityType { get; set; }
        public Int64 AmountGuaranteedByThirdParty { get; set; }
        public Int64 AmountGuaranteedBySecurityType { get; set; }
        public string QualitativeJudgement { get; set; }
        public DateTime? DateOfClassification { get; set; }
        public DateTime? DateOfLawSuit { get; set; }
        public string EconomicPurposeCode { get; set; }
        public Int64 CumulativeRecovery { get; set; }
        public Int64 RecoveryDuringTheReportingPeriod { get; set; }
        public Int64 DueForRecovery { get; set; }
        public Int64 TotalOutstandingAmount { get; set; }

        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string RecordStatus { get; set; }

    }
}