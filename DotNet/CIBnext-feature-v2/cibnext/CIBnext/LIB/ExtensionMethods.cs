using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;


namespace CIBnext.LIB
{
    public static class ExtensionMethods
    {
        public static bool TryGetOrdinal(this IDataRecord dr, string colname, out int ord)
        {
            try
            {
                ord = dr.GetOrdinal(colname);
                return true;
            }
            catch
            {
                ord = -1;
                return false;
            }
        }

        public static bool IsInRoles(this IPrincipal user, params string[] roles)
        {
            foreach (var role in roles)
                if (user.IsInRole(role))
                    return true;
            return false;
        }


        private static string TitleCaseType(string type)
        {
            return type.Substring(0, 1).ToUpper() + type.Substring(1);
        }
   }
}