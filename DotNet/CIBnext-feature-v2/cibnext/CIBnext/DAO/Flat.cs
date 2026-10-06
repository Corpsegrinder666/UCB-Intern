using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Flat
    {
        public string RecordType { get; set; }
        public string FiCode { get; set; }
        public string BranchCode { get; set; }
        public string SecurityValueCode { get; set; }
        public string SecurityCategory { get; set; }
        public string FiSecurityCode { get; set; }
        public string TypeDeed { get; set; }
        public string TitleDeedPoaNo { get; set; }
        public DateTime DateRegistration { get; set; }
        public string NameSubRegistryOffice { get; set; }
        public string District { get; set; }
        public string Thana { get; set; }
        public string Mouza { get; set; }
        public string TripartiteAgreement { get; set; }
        public int AreaFlat { get; set; }
        public decimal UndemarcatedTotalLandArea { get; set; }
        public string ApartmentFlatNo { get; set; }
        public int FloorNo { get; set; }
        public string LocationFlat { get; set; }
        public string CityCorprationHoldingNo { get; set; }
        public string NameBuilding { get; set; }
        public string NameProject { get; set; }
        public string NameDeveloper { get; set; }
        public string RehabMemberNoDeveloper { get; set; }
        public string Address { get; set; }
        public string JoteNo { get; set; }
        public string DagNoCs { get; set; }
        public string DagNoSa { get; set; }
        public string DagNoRs { get; set; }
        public string DagNoBs { get; set; }
        public string DagNoCityJorip { get; set; }
        public string KhatianNoCs { get; set; }
        public string KhatianNoSa { get; set; }
        public string KhatianNoRs { get; set; }
        public string KhatianNoBs { get; set; }
        public string KhatianNoCityJorip { get; set; }
        public string MutationKhatianNo { get; set; }
        public string LeaseholdProperty { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string CheckBy { get; set; }
        public DateTime? CheckDate { get; set; }
        public string RecordStatus { get; set; }
        public string SubjectName { get; set; }
        public string SubjectRole { get; set; }
    }
}