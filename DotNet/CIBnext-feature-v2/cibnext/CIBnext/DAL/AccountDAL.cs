using CIBnext.DAO;
using CIBnext.LIB;
using Microsoft.Extensions.Configuration;
using NLog;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;

namespace CIBnext.DAL
{
    public class AccountDAL
    {
        private readonly IConfiguration _configuration;
        private Logger _logger = LogManager.GetLogger("AccountDAL");

        public AccountDAL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AppUser SelectUserByAd(string username)
        {
            AppUser user = null;
            try
            {
                using (OracleConnection con = new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_ACCOUNT.SP_GET_APP_USER_BY_AD";
                    com.Parameters.Add("ADID_IN", username);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        if (dr.Read())
                        {
                            user = new AppUser();
                            var ord = 0;
                            if (dr.TryGetOrdinal("EMPID", out ord) && !dr.IsDBNull(ord))
                                user.EmployeeID = dr.GetInt32(ord);
                            if (dr.TryGetOrdinal("ADID", out ord) && !dr.IsDBNull(ord))
                                user.DomainID = dr.GetString(ord);
                            if (dr.TryGetOrdinal("FULLNAME", out ord) && !dr.IsDBNull(ord))
                                user.FullName = dr.GetString(ord);
                            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                                user.BranchCode = dr.GetString(ord);
                            if (dr.TryGetOrdinal("BRANCH_NAME", out ord) && !dr.IsDBNull(ord))
                                user.BranchName = dr.GetString(ord);
                            if (dr.TryGetOrdinal("AD", out ord) && !dr.IsDBNull(ord))
                                user.AD = dr.GetString(ord);
                            if (dr.TryGetOrdinal("CIB_BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                                user.CibBranchCode = dr.GetString(ord);
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return user;
        }

        public string[] SelectUserRolesByAd(string username)
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con =
           new OracleConnection(_configuration.GetConnectionString("OracleConnection")))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "PKG_ACCOUNT.SP_GET_USER_ROLES_BY_AD";

                    com.Parameters.Add("ADID_IN", username);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(dr.GetString(0));

                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list.ToArray();
        }
    }
}