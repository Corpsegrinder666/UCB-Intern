using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class InstalmentContract : InstalmentContractBase, INonCard
    {
        public Int64 SanctionLimit { get; set; }
        public Int64 TotalDisbursedAmount { get; set; }
        public int TotalNumberOfInstallments { get; set; }
        public Int64 AmountOfNextExpiringInstallment { get; set; }
        public int NumberOfRemainingInstallments { get; set; }
        public string TypeOfLeasedGood { get; set; }
        public Int64 ValueOfLeasedGood { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime DateOfManufacturing { get; set; }
        public string NumberOfTimesRescheduling { get; set; }
        public DateTime DateOfLastRescheduling { get; set; }
        public string SME { get; set; }
        public string EnterpriseType { get; set; }
    }
}