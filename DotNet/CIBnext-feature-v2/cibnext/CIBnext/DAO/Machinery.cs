using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Machinery
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string SecurityValueCode { get; set; }
        public string SecurityCategory { get; set; }
        public string FiSecurityCode { get; set; }
        public string NameMachinery { get; set; }
        public string NameFactory { get; set; }
        public string AddressFactory { get; set; }
        public string MfgCoBrandName { get; set; }
        public string MfgCountry { get; set; }
        public int MfgYear { get; set; }
        public string ModelNo { get; set; }
        public int NoUnit { get; set; }
        public string LcNo { get; set; }
        public DateTime LcDate { get; set; }
        public int ValueLc { get; set; }
        public string LadingAirWayBillNo { get; set; }
        public int PresentValue { get; set; }
        public int BookInvoiceValue { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string CheckBy { get; set; }
        public DateTime CheckDate { get; set; }
        public string RecordStatus { get; set; }
        public string SubjectName { get; set; }
        public string SubjectRole { get; set; }
    }
}