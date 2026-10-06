using CIBnext.DAL;
using CIBnext.DAO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIBnext.BLL
{
    public class MiscBLL
    {
        private MiscDAL _miscDal;

        public MiscBLL(MiscDAL miscDal)
        {
            _miscDal = miscDal;
        }
        public async Task<ReportingParameters> GetParameters()
        {
            return await _miscDal.SelectParameters();
        }

        internal async Task<List<Branch>> GetBranches()
        {
            return await _miscDal.SelectBranches();
        }
    }
}