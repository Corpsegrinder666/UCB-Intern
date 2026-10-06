using CIBnext.DAO;
using CIBnext.LIB;

using Dapper;

using NLog;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAL
{
    public class MisDAL
    {
        private readonly Logger _logger = LogManager.GetLogger("MisDAL");

        public IEnumerable<string> SelectLatestCifs(DateTime monthStart, DateTime loanDate, string branchCode)
        {
            try
            {
                var sql = @"
SELECT
    distinct c.CUSTOMER_ID
    -- opening date ?
    -- c.VALUE_DATE,
    -- c.MATURITY_DATE,
    -- c.LINE_EXPIRY_DATE,
    -- c.GENERATION_DATE,
    -- c.LAST_RESCHEDULE_DATE,
FROM
    ucbprod.ucb_loan_list_hist_new c WHERE generation_date >= to_date(:generation_date, 'YYYY-MM-DD')
AND to_date(c.generation_date, 'DD-MM-YYYY') <= to_date(:opening_date, 'YYYY-MM-DD')
AND BRANCH_CODE = NVL(:branch_code, BRANCH_CODE)
";
                var odp = new OracleDynamicParameters();
                odp.Add(":generation_date", loanDate.ToString("yyyy-MM-dd"));
                odp.Add(":opening_date", monthStart.ToString("yyyy-MM-dd"));
                odp.Add(":branch_code", branchCode);

                using (OracleConnection con = new OracleConnection(WebConfig.UcbMisConnectionString))
                {
                    var res = con.Query<string>(sql, odp);
                    return res;
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<LoanRecord> SelectLoansForCifs(DateTime loanDate, IEnumerable<string> cifs, string branchCode)
        {
            try
            {
                var loans = new List<LoanRecord>();
                var sql = @"
SELECT
    CUSTOMER_ID CIF,
    BRANCH_CODE BranchCode,
    CUSTOMER_NAME CustomerName,
    CONT_REF_NO LoanAcNumber,
    to_date(VALUE_DATE, 'DD-MM-YYYY') OpeningDate,
    OUTSTANDING_LOCAL_CURRENCY OutstandingAmount,
    STATUS1 ClassificationStatus
FROM
    ucbprod.ucb_loan_list_hist_new c WHERE generation_date = to_date(:generation_date, 'YYYY-MM-DD')
AND BRANCH_CODE = NVL(:branch_code, BRANCH_CODE)
AND customer_id in :cifs
";

                using (OracleConnection con = new OracleConnection(WebConfig.UcbMisConnectionString))
                {
                    for (int i = 0; i <= cifs.Count() / 1000; i++)
                    {
                        var odp = new OracleDynamicParameters();
                        odp.Add(":generation_date", loanDate.ToString("yyyy-MM-dd"));
                        odp.Add(":branch_code", branchCode);
                        odp.Add(":cifs", cifs.Skip(i * 1000).Take(1000));
                        var res = con.Query<LoanRecord>(sql, odp);
                        loans.AddRange(res);
                    }
                    return loans;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return Enumerable.Empty<LoanRecord>();
            }
        }
    }
}