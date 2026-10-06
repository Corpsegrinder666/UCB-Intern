using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class LoanRecord
    {
        public string BranchCode { get; set; }
        public string CIF { get; set; }
        public string CustomerName { get; set; }
        public string LoanAcNumber { get; set; }
        public DateTime OpeningDate { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string ClassificationStatus { get; set; }
    }
}