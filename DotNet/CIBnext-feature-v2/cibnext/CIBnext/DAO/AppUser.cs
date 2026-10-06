using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class AppUser
    {
        public int EmployeeID { get; set; }
        public string DomainID { get; set; }
        public string FullName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string CibBranchCode { get; set; }
        public string AD { get; set; }
        public ReportingParameters Parameters { get; set; }
    }
}