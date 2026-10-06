using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class OwnerLink
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string CibBranchCode { get; set; }
        public string InstitutionFiSubjectCode { get; set; }
        public string InstitutionName { get; set; }
        public string Role { get; set; }
        public string OwnerFiSubjectCode { get; set; }
        public string OwnerName { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string RecordStatus { get; set; }
        public string OwnerFiSubjectName { get; set; }
        public string InstitutionFiSubjectName { get; set; }
    }
}