using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Extensions.Configuration;
using NLog;
using CIBnext.DAO;
using System.Threading.Tasks;
using Dapper;
using CIBnext.LIB;
using System.Data;

namespace CIBnext.DAL
{
    public class MiscDAL
    {
        private readonly IConfiguration _configuration;
        private Logger _logger = LogManager.GetLogger("MiscDAL");
        public MiscDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ReportingParameters> SelectParameters()
        {
            ReportingParameters param = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    IEnumerable<KeyValuePair<string, string>> items = await con.QueryAsync<KeyValuePair<string, string>>(
                        "pkg_misc.SP_GET_PARAMETERS",
                        dp,
                        commandType: CommandType.StoredProcedure);
                    if(items != null)
                    {
                        param = new ReportingParameters();
                        foreach(KeyValuePair<string, string> item in items)
                        {
                            if (item.Key.Equals("REPORTING_PERIOD"))
                                param.ReportingPeriod = DateTime.Parse(item.Value);
                            else if (item.Key.Equals("CIB_RPT_RUNNING"))
                                param.IsCibReportingRunning = item.Value.Equals("Y");
                            else if (item.Key.Equals("SEC_RPT_RUNNING"))
                                param.IsSecurityReportingRunning = item.Value.Equals("Y");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return param;
        }

        public async Task<List<Branch>> SelectBranches()
        {
            IEnumerable<Branch> branches = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                    branches = await con.QueryAsync<Branch>("PKG_MISC.SP_GET_BRANCHES", dp, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            if (branches == null)
                return new List<Branch>();
            else
                return branches.AsList();
        }
    }
}