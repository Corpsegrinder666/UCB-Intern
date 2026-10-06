using CIBnext.DAL;
using CIBnext.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace CIBnext.BLL
{
    public class UbsBLL
    {
        private UbsDAL _ubsDal = new UbsDAL();

        public async Task<UbsCif> GetCustomerInfo(string customerNo)
        {
            var custInfo = await _ubsDal.SelectCustomerInfo(customerNo);
            if (custInfo is null)
                return custInfo;
            return CleanupCustInfo(custInfo);
        }

        private readonly Regex spaceRegex = new Regex(" +");
        private UbsCif CleanupCustInfo(UbsCif custInfo)
        {
            custInfo.Name = spaceRegex.Replace(custInfo.Name ?? "", " ").Trim();
            custInfo.FatherName = spaceRegex.Replace(custInfo.FatherName ?? "", " ").Trim();
            custInfo.MotherName = spaceRegex.Replace(custInfo.MotherName ?? "", " ").Trim();
            custInfo.Address = spaceRegex.Replace(custInfo.Address ?? "", " ").Trim();
            return custInfo;
        }

        internal async Task<UbsAccountInfo> GetAccountInfo(string accountNo)
        {
            return await _ubsDal.SelectAccountInfo(accountNo);
        }
    }
}