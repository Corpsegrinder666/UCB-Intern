using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class ContractNeedTobeReportedInfo
    {
        public string BranchName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string AccountNumber { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal AccountLimit { get; set; }
        public decimal OutstandingAmount { get; set; }
        public decimal Overdue { get; set; }
        public int OverdueDays { get; set; }
        public decimal AmountPaidLastMonth { get; set; }
        public decimal TotalAmountPaid { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Status { get; set; }
        public string NumberOfTimesRescheduled { get; set; }
        public DateTime? LastRescheduleDate { get; set; }
        public string ProductCode { get; set; }
        public string ProductDescription { get; set; }
        public decimal SecurityValue { get; set; }
        public string SectorCode { get; set; }
        public string EconomicPurposeCode { get; set; }
        public DateTime? LastClassificationDate { get; set; }
        public string SmeCode { get; set; }
        public decimal EmiAmount { get; set; }
        public decimal InstallmentSizeForCL { get; set; }
        public string InstallmentFrequency { get; set; }
        public int NoOfInstallmentDue { get; set; }
        public DateTime? LawSuitDate { get; set; }
        public int NoOfInstallmentPaid { get; set; }
        public string InstalmentFiContractCode { get; set; }
        public string InstalmentFiSubjectCode { get; set; }
        public int? TotalNoOfInstalment { get; set; }
        public int? NoOfRemainingInstalment { get; set; }
        public int? NoOfOverdueInstalment { get; set; }
        public string NonInstalmentFiContractCode { get; set; }
        public string NonInstalmentFiSubjectCode { get; set; }
    }
}