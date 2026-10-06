using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class PersonalData
    {
        public string RecordType { get; set; }
        public string FICode { get; set; }
        public string BranchCode { get; set; }
        public string FISubjectCode { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string FathersTitle { get; set; }
        public string FathersName { get; set; }
        public string MothersTitle { get; set; }
        public string MothersName { get; set; }
        public string SpousesTitle { get; set; }
        public string SpousesName { get; set; }
        public int SectorType { get; set; }
        public int SectorCode { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string CountryOfBirth { get; set; }
        public string NationalIDNumber { get; set; }
        public int NationalIDAvailable { get; set; }
        public string TIN { get; set; }
        public Address PermanentAddress { get; set; }
        public Address PresentAddress { get; set; }
        public Address BusinessAddress { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public DateTime IDIssueDate { get; set; }
        public string IDIssueCountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string MakeBy { get; set; }
        public DateTime MakeDate { get; set; }
        public string RecordStatus { get; set; }
        public string CifNo { get; set; }
        public string SmartIDNumber { get; set; }

        //meta information
        public bool IsBorrower { get; set; }
        public bool IsMortgagor { get; set; }
    }
}