using CIBnext.DAL;
using CIBnext.DAO;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIBnext.BLL
{
    public class ReportBLL
    {
        private readonly ReportDAL _rptDal;

        public ReportBLL(IConfiguration configuration)
        {
            _rptDal = new ReportDAL(configuration);
        }

        public async Task<IEnumerable<ContractReadyToReportInfo>> GetContractsReadyToReport(string cibBranchCode, string contractPhase, string contractStatus)
        {
            return await _rptDal.SelectContractsReadyToReport(cibBranchCode, contractPhase, contractStatus);
        }

        public async Task<DateTime?> GetLastValidatorRunDate()
        {
            return await _rptDal.SelectLastValidatorRunDate();
        }

        public async Task<IEnumerable<ContractNeedTobeReportedInfo>> GetContractsNeedTobeReported(string cibBranchCode)
        {
            return await _rptDal.SelectContractsNeedTobeReported(cibBranchCode);
        }
    }
}