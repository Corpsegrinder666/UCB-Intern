using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class SecurityLink
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string LinkType { get; set; }
        public string FiSecurityCode { get; set; }
        public string FiMortHypoCode { get; set; }
    }
}