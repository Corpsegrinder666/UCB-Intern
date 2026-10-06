using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class UbsCif
    {
        public string CustomerNo { get; set; }
        public string CustomerType { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryBirth { get; set; }
        public string Address { get; set; }
        public string AddressCountry { get; set; }
    }
}