using CIBnext.DAL;
using CIBnext.DAO;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.BLL
{
    public class MisBLL
    {
        private readonly MisDAL _misDal = new MisDAL();

        public IEnumerable<string> GetLatestCifs(DateTime monthStart, DateTime loanDate, string branchCode)
        {
            return _misDal.SelectLatestCifs(monthStart, loanDate, branchCode);
        }

        public IEnumerable<LoanRecord> GetLoansForCifs(DateTime loanDate, IEnumerable<string> cifs, string branchCode)
        {
            return _misDal.SelectLoansForCifs(loanDate, cifs, branchCode);
        }
    }
}