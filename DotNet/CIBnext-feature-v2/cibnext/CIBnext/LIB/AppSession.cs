using CIBnext.DAO;
using System.Security.Claims;

namespace CIBnext.LIB
{
    public static class AppSession
    {
        public static AppUser GetUserInfo(ClaimsPrincipal principle)
        {
            var userInfo = new AppUser();

            if (principle == null)
                return userInfo;

            userInfo.DomainID =
                principle.FindFirst("ADID")?.Value ?? "";

            userInfo.FullName =
                principle.FindFirst("FullName")?.Value ?? "";

            userInfo.BranchCode =
                principle.FindFirst("BranchCode")?.Value ?? "";

            userInfo.BranchName =
                principle.FindFirst("BranchName")?.Value ?? "";

            userInfo.CibBranchCode =
                principle.FindFirst("CibBranchCode")?.Value ?? "";

            if (int.TryParse(
                principle.FindFirst("EmployeeId")?.Value,
                out int empId))
            {
                userInfo.EmployeeID = empId;
            }

            var reportingPeriod =
                principle.FindFirst("ReportingPeriod")?.Value;

            if (!string.IsNullOrEmpty(reportingPeriod))
            {
                userInfo.Parameters = new ReportingParameters
                {
                    ReportingPeriod = DateTime.Parse(reportingPeriod)
                };
            }

            return userInfo;
        }

        public static ReportingParameters Parameters
        {
            get
            {
                return null;
            }
        }
    }
}