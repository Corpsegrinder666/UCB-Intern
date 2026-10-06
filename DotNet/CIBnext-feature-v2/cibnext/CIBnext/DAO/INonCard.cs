using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public interface INonCard
    {
        Int64 SanctionLimit { get; set; }
        string NumberOfTimesRescheduling { get; set; }
        DateTime DateOfLastRescheduling { get; set; }
        string SME { get; set; }
        string EnterpriseType { get; set; }
    }
}