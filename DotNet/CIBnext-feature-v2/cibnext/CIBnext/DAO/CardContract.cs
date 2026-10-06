using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class CardContract : InstalmentContractBase
    {
        public Int64 CreditLimit { get; set; }
        public DateTime DateOfLastCharge { get; set; }
        public string TypeOfInstallment { get; set; }
    }
}