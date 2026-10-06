using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.DAO
{
    public class ReportingParameters
    {
        public DateTime ReportingPeriod { get; set; }
        public bool IsCibReportingRunning { get; set; }
        public bool IsSecurityReportingRunning { get; set; }
    }
}