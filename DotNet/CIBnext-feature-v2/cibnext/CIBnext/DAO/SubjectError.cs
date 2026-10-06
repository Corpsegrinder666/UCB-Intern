using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class SubjectError
    {
        public Int64 VersionNo { get; set; }
        public DateTime ReportingPeriod { get; set; }
        public string FiSubjectCode { get; set; }
        public string FiSubjectName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
    }
}