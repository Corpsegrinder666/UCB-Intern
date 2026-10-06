using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class ContractError
    {
        public Int64 VersionNo { get; set; }
        public DateTime ReportingPeriod { get; set; }
        public string FiContractCode { get; set; }
        public string FiPrimaryCode { get; set; }
        public string FiPrimaryName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
    }
}