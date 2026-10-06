using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class ContractReadyToReportInfo
    {
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string UbsAccountNo { get; set; }
        public string FiContractCode { get; set; }
        public string ContractType { get; set; }
        public string CifNo { get; set; }
        public string FiSubjectCode { get; set; }
        public string SubjectName  { get; set; }
        public string CustomerName { get; set; }
        public DateTime? StartingDate { get; set; }
        public decimal TotalDisbursedAmount { get; set; }
        public string ContractPhase { get; set; }
        public decimal OverdueAmount { get; set; }
        public int NoOfOverdueInstalment { get; set; }
        public int NoOfRemainingInstalment { get; set; }
        public decimal TotalOutstandingAmount { get; set; }
        public string I_NI { get; set; }
        public string ContractStatus { get; set; }
        public string CLStatus { get; set; }
        public DateTime? DateOfClassification { get; set; }
        public DateTime? PlannedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public string Remarks { get; set; }
        public string DefaulterStatus { get; set; }
        public decimal TotalInstallmentNo { get; set; }
        public decimal InstallmentAmount { get; set; }
        public string NoOfTimesRescheduling { get; set; }
        public DateTime? DateOfLastRescheduling { get; set; }
        public string RecordStatus { get; set; }
    }
}
