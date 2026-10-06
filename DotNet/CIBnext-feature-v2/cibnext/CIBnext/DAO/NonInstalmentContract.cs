using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class NonInstalmentContract : Contract, INonCard
    {
        public Int64 SanctionLimit { get; set; }
        public int NumberOfDaysOfPaymentDelay { get; set; }
        public string NumberOfTimesRescheduling { get; set; }
        public DateTime DateOfLastRescheduling { get; set; }
        public string SME { get; set; }
        public string EnterpriseType { get; set; }
    }
}