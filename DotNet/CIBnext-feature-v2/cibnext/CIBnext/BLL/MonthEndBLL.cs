using CIBnext.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.BLL
{
    public class MonthEndBLL
    {
        MonthEndDAL dal = new MonthEndDAL();
        public bool LockSubjects()
        {
            return dal.UpdateSubjectsToLock();
        }

        public bool LockLinks()
        {
            return dal.UpdateLinksToLock();
        }

        public bool ClearErrors()
        {
            return dal.TruncateErrorTables();
        }

        public bool TerminateTMTAContracts()
        {
            return dal.UpdateTMTAContractsToTerminate();
        }
    }
}