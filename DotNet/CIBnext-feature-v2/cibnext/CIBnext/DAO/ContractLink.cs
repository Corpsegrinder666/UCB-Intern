using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class ContractLink
    {
        public string RecordType { get; set; }
        public string FICode { get; set; }
        public string BranchCode { get; set; }
        public string TypeOfLink { get; set; }
        public string FIPrimaryCode { get; set; }
        public string FIPrimaryName { get; set; }
        public string FISecondaryCode { get; set; }
        public string FISecondaryName { get; set; }
        public string FIContractCode { get; set; }
        public string ContractCategory { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string RecordStatus { get; set; }
    }
}