using CIBnext.DAO;
using CIBnext.LIB;

using Dapper;

using Microsoft.Extensions.Configuration;

using NLog;

using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CIBnext.DAL
{
    public class ReportDAL
    {
        private readonly IConfiguration _configuration;
        public readonly Logger _logger = LogManager.GetLogger("ReportDAL");

        public ReportDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ContractReadyToReportInfo>> SelectContractsReadyToReport(string cibBranchCode, string contractPhase, string contractStatus)
        {
            try
            {
                using (var conn = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_CIB_BRANCH_CODE", cibBranchCode);
                    dp.Add("IN_CONTRACT_PHASE", string.IsNullOrEmpty(contractPhase) ? null : contractPhase);
                    dp.Add("IN_CONTRACT_STATUS", string.IsNullOrEmpty(contractStatus) ? null : contractStatus);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    var res = await conn.QueryAsync<ContractReadyToReportInfo>("PKG_REPORT.SP_ACCS_READY_TO_REPORT", dp,
                        commandType: System.Data.CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SelectContractsReadyToReport");
            }
            return Array.Empty<ContractReadyToReportInfo>();
        }

        public async Task<IEnumerable<ContractNeedTobeReportedInfo>> SelectContractsNeedTobeReported(string cibBranchCode)
        {
            try
            {
                using (var conn = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    OracleDynamicParameters dp = new OracleDynamicParameters();
                    dp.Add("IN_CIB_BRANCH_CODE", cibBranchCode);
                    dp.Add("REF_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output);

                    var res = await conn.QueryAsync<ContractNeedTobeReportedInfo>("PKG_REPORT.SP_NEED_TOBE_REPORTED", dp,
                        commandType: System.Data.CommandType.StoredProcedure);

                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SelectContractsNeedTobeReported");
            }
            return Array.Empty<ContractNeedTobeReportedInfo>();
        }

        public async Task<DateTime?> SelectLastValidatorRunDate()
        {
            try
            {
                string sql = "select log_time from (select log_time from validator_log order by log_id desc) a where rownum = 1";
                using (var conn = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                {
                    var res = await conn.QueryFirstOrDefaultAsync<DateTime?>(sql);
                    return res;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in SelectLastValidatorRunDate");
            }
            return null;
        }
    }
}