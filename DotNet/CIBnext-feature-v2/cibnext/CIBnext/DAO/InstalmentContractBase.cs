using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class InstalmentContractBase : Contract
    {
        public string PeriodicityOfPayment { get; set; }
        public string MethodOfPayment { get; set; }
        public Int64 InstalmentAmount { get; set; }
        public DateTime? ExpirationDateOfNextInstallment { get; set; }
        public Int64 RemainingAmount { get; set; }
        public int NumberOfOverdueInstallment { get; set; }
        public Int64 OverdueAmount { get; set; }
        public int NumberOfDaysOfPaymentDelay { get; set; }
    }
}