using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Admin
    {
        public int EmployeeID { get; set; }
        public string DomainID { get; set; }
        public string FullName { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string CibBranchCode { get; set; }
        public string ADID { get; set; }

        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }

        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }

        public string Status { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        
    }
}