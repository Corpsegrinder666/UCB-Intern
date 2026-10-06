using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class SubjectLink
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string LinkType { get; set; }
        public string FiSubjectCode { get; set; }
        public string FiSecMortHypoCode { get; set; }
        public string Role { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string CheckBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public string RecordStatus { get; set; }
    }
}