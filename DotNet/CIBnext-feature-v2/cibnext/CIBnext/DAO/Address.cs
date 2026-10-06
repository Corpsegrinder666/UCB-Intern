using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class Address
    {
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string District { get; set; }
        public string CountryCode { get; set; }
    }
}