using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Institution
    {
        public string RecordType { get; set; }
        public string FICode { get; set; }
        public string BranchCode { get; set; }
        public string FISubjectCode { get; set; }
        public string Title { get; set; }
        public string TradeName { get; set; }
        public int SectorType { get; set; }
        public int SectorCode { get; set; }
        public int LegalForm { get; set; }
        public string LegalFormText { get; set; }
        public string RegistrationNumberRJSC { get; set; }
        public DateTime RegistrationDateRJSC { get; set; }
        public string TIN { get; set; }
        public Address BusinessAddress { get; set; }
        public Address FactoryAddress { get; set; }
        public int CRGScoring { get; set; }
        public int CreditRating { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string RecordStatus { get; set; }
        public string CifNo { get; set; }

        //meta information
        public bool IsBorrower { get; set; }
        public bool IsMortgagor { get; set; }
    }
}