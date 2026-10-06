using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CIBnext.DAO;
using CIBnext.LIB;
using Dapper;
using NLog;
using Oracle.ManagedDataAccess.Client;

namespace CIBnext.DAL
{
    public class UbsDAL
    {
        private Logger _logger = LogManager.GetLogger("UbsDAL");

        public async Task<UbsCif> SelectCustomerInfo(string customerNo)
        {
            UbsCif cif = null;
            string sql =
@"select 
a.customer_no ""CustomerNo"",
a.customer_type ""CustomerType"",
a.full_name ""Name"",
nvl(a.address_line1 || ' ', '') || nvl(a.address_line2 || ' ', '') ||
nvl(a.address_line3 || ' ', '') || nvl(a.address_line4 || ' ', '') ""Address"",
a.country ""AddressCountry"",
c.father_name ""FatherName"",
c.mother_name ""MotherName"",
b.sex ""Gender"",
b.date_of_birth ""DateOfBirth"",
b.birth_country ""CountryBirth""
from fcubs147.stzm_customer a
join fcubs147.stzm_cust_personal b on a.customer_no = b.customer_no
join fcubs147.sttm_cust_personal_custom c on a.customer_no = c.customer_no
where a.customer_no = :customer_no";
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UBSConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add(":customer_no", customerNo);
                    cif = await con.QueryFirstOrDefaultAsync<UbsCif>(sql, dp);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return cif;
        }

        internal async Task<UbsAccountInfo> SelectAccountInfo(string accountNo)
        {
            UbsAccountInfo acInfo = null;
            string sql = 
@"SELECT a.CONTRACT_REF_NO ""AccountNo"" FROM (
select CONTRACT_REF_NO from fcubs147.cszb_contract WHERE contract_ref_no = :ac
UNION
select ACCOUNT_NUMBER from fcubs147.cltb_account_master WHERE account_number = :ac
union
select CUST_AC_NO from fcubs147.stzm_cust_account
where substr(cust_ac_no, 5, 1) = '7' AND cust_ac_no = :ac) a";
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UBSConnectionString))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add(":ac", accountNo);
                    acInfo = await con.QueryFirstOrDefaultAsync<UbsAccountInfo>(sql, dp);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return acInfo;
        }
    }
}