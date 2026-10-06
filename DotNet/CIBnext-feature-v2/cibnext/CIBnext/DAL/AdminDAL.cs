using NLog;
using System;
using Oracle.ManagedDataAccess.Client;
using CIBnext.DAO;
using System.Data;
using CIBnext.LIB;
using System.Collections.Generic;

namespace CIBnext.DAL
{
    public class AdminDAL
    {
        public AdminDAL()
        {
        }

        private Logger _logger = LogManager.GetLogger("AdminDAL");


        #region User Info Page

        public List<Admin> SelectUsersByFilters(string empId, string empName, string branchCode)
        {
            List<Admin> list = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_USERS_BY_FILTERS";
                    com.Parameters.Add("EMP_ID_IN", string.IsNullOrEmpty(empId) ? (object)DBNull.Value : (object)empId);
                    com.Parameters.Add("EMP_NAME_IN", string.IsNullOrEmpty(empName) ? (object)DBNull.Value : (object)empName);
                    com.Parameters.Add("BRANCH_CODE_IN", string.IsNullOrEmpty(branchCode) ? (object)DBNull.Value : (object)branchCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortUserRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }


        private Admin FillShortUserRecord(OracleDataReader dr)
        {
            Admin user = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("EMPID", out ord) && !dr.IsDBNull(ord))
                user.EmployeeID = Convert.ToInt32(dr[ord].ToString());
            if (dr.TryGetOrdinal("ADID", out ord) && !dr.IsDBNull(ord))
                user.ADID = dr.GetString(ord);
            if (dr.TryGetOrdinal("FULLNAME", out ord) && !dr.IsDBNull(ord))
                user.FullName = dr.GetString(ord);
            if (dr.TryGetOrdinal("BRANCH_CODE", out ord) && !dr.IsDBNull(ord))
                user.BranchCode = dr.GetString(ord);

            return user;
        }


        public bool InsertUserInfo(Admin user)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_USERINFO_INSERT";
                    com.Parameters.Add("EMP_ID_IN", user.EmployeeID);
                    com.Parameters.Add("AD_ID_IN", user.ADID);
                    com.Parameters.Add("FULLNAME_IN", user.FullName);
                    com.Parameters.Add("BRANCH_CODE_IN", user.BranchCode);
                    com.Parameters.Add("STATUS_IN", user.Status);
                    com.Parameters.Add("AD_IN", user.DomainID);
                    com.Parameters.Add("MAKE_BY_IN", user.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", user.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteUser(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_USERINFO_DELETE";
                        com.Parameters.Add("EMP_ID_IN", OracleDbType.Int32);
                        com.Parameters.Add("STATUS_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["EMP_ID_IN"].Value = subCode.EmployeeID;
                                com.Parameters["STATUS_IN"].Value = subCode.Status;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


        #region Roles Page

        public List<Admin> SelectRolesByFilters(string roleCode, string roleDesc)
        {
            List<Admin> listRole = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_ROLES_BY_FILTERS";
                    com.Parameters.Add("ROLE_CODE_IN", string.IsNullOrEmpty(roleCode) ? (object)DBNull.Value : (object)roleCode);
                    com.Parameters.Add("DESCRIPTION_IN", string.IsNullOrEmpty(roleDesc) ? (object)DBNull.Value : (object)roleDesc);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            listRole.Add(FillShortRolesRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return listRole;
        }


        private Admin FillShortRolesRecord(OracleDataReader dr)
        {
            Admin role = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("ROLE_CODE", out ord) && !dr.IsDBNull(ord))
                role.RoleCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("DESCRIPTION", out ord) && !dr.IsDBNull(ord))
                role.RoleDescription = dr.GetString(ord);

            return role;
        }


        public bool InsertRoleInfo(Admin role)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_ROLE_INSERT";
                    com.Parameters.Add("ROLE_CODE_IN", role.RoleCode);
                    com.Parameters.Add("DESCRIPTION_IN", role.RoleDescription);
                    com.Parameters.Add("STATUS_IN", role.Status);
                    com.Parameters.Add("MAKE_BY_IN", role.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", role.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteRole(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_ROLE_DELETE";
                        com.Parameters.Add("ROLE_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("STATUS_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["ROLE_CODE_IN"].Value = subCode.RoleCode;
                                com.Parameters["STATUS_IN"].Value = subCode.Status;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


        #region Group Page

        public List<Admin> SelectGroupsByFilters(string GroupCode, string GroupDesc)
        {
            List<Admin> listGroup = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_GROUP_BY_FILTERS";
                    com.Parameters.Add("GROUP_CODE_IN", string.IsNullOrEmpty(GroupCode) ? (object)DBNull.Value : (object)GroupCode);
                    com.Parameters.Add("DESCRIPTION_IN", string.IsNullOrEmpty(GroupDesc) ? (object)DBNull.Value : (object)GroupDesc);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            listGroup.Add(FillShortGroupRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return listGroup;
        }


        private Admin FillShortGroupRecord(OracleDataReader dr)
        {
            Admin group = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("GROUP_CODE", out ord) && !dr.IsDBNull(ord))
                group.GroupCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("DESCRIPTION", out ord) && !dr.IsDBNull(ord))
                group.GroupDescription = dr.GetString(ord);

            return group;
        }


        public bool InsertGroupInfo(Admin role)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GROUP_INSERT";
                    com.Parameters.Add("GROUP_CODE_IN", role.GroupCode);
                    com.Parameters.Add("DESCRIPTION_IN", role.GroupDescription);
                    com.Parameters.Add("STATUS_IN", role.Status);
                    com.Parameters.Add("MAKE_BY_IN", role.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", role.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteGroup(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_GROUP_DELETE";
                        com.Parameters.Add("GROUP_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("STATUS_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["GROUP_CODE_IN"].Value = subCode.GroupCode;
                                com.Parameters["STATUS_IN"].Value = subCode.Status;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


        #region User Role Page

        public List<Admin> SelectUserRoleByFilters(string empId, string roleCode)
        {
            List<Admin> list = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_USERROLE_BY_FILTERS";
                    com.Parameters.Add("EMP_ID_IN", string.IsNullOrEmpty(empId) ? (object)DBNull.Value : (object)empId);
                    com.Parameters.Add("ROLE_CODE_IN", string.IsNullOrEmpty(roleCode) ? (object)DBNull.Value : (object)roleCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortUserRoleRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private Admin FillShortUserRoleRecord(OracleDataReader dr)
        {
            Admin userRole = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("EMPID", out ord) && !dr.IsDBNull(ord))
                userRole.EmployeeID = Convert.ToInt32(dr[ord].ToString());
            if (dr.TryGetOrdinal("ROLE_CODE", out ord) && !dr.IsDBNull(ord))
                userRole.RoleCode = dr.GetString(ord);

            return userRole;
        }

        public List<string> GetDdlEmpId_UserRole()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_EMPID";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["EMPID"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public List<string> GetDdlRoleCode_UserRole()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_ROLECODE";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["ROLE_CODE"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }


        public bool InsertUserRoleInfo(Admin userRole)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_USERROLE_INSERT";
                    com.Parameters.Add("EMP_ID_IN", userRole.EmployeeID);
                    com.Parameters.Add("ROLE_CODE_IN", userRole.RoleCode);
                    com.Parameters.Add("MAKE_BY_IN", userRole.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", userRole.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteUserRole(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_USERROLE_DELETE";
                        com.Parameters.Add("EMP_ID_IN", OracleDbType.Int32);
                        com.Parameters.Add("ROLE_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["EMP_ID_IN"].Value = subCode.EmployeeID;
                                com.Parameters["ROLE_CODE_IN"].Value = subCode.RoleCode;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


        #region User Group Page

        public List<Admin> SelectUserGroupByFilters(string empId, string groupCode)
        {
            List<Admin> list = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_USERGROUP_BY_FILTERS";
                    com.Parameters.Add("EMP_ID_IN", string.IsNullOrEmpty(empId) ? (object)DBNull.Value : (object)empId);
                    com.Parameters.Add("GROUP_CODE_IN", string.IsNullOrEmpty(groupCode) ? (object)DBNull.Value : (object)groupCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortUserGroupRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private Admin FillShortUserGroupRecord(OracleDataReader dr)
        {
            Admin userRole = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("EMPID", out ord) && !dr.IsDBNull(ord))
                userRole.EmployeeID = Convert.ToInt32(dr[ord].ToString());
            if (dr.TryGetOrdinal("GROUP_CODE", out ord) && !dr.IsDBNull(ord))
                userRole.GroupCode = dr.GetString(ord);

            return userRole;
        }

        public List<string> GetDdlEmpId_UserGroup()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_EMPID_USERGROUP";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["EMPID"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public List<string> GetDdlGroupCode_UserGroup()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_GROUPCODE_USERGROUP";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["GROUP_CODE"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }


        public bool InsertUserGroupInfo(Admin userGroup)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_USERGROUP_INSERT";
                    com.Parameters.Add("EMP_ID_IN", userGroup.EmployeeID);
                    com.Parameters.Add("GROUP_CODE_IN", userGroup.GroupCode);
                    com.Parameters.Add("MAKE_BY_IN", userGroup.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", userGroup.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteUserGroup(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_USERGROUP_DELETE";
                        com.Parameters.Add("EMP_ID_IN", OracleDbType.Int32);
                        com.Parameters.Add("GROUP_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["EMP_ID_IN"].Value = subCode.EmployeeID;
                                com.Parameters["GROUP_CODE_IN"].Value = subCode.GroupCode;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


        #region Group Role Page

        public List<Admin> SelectGroupRoleByFilters(string groupCode, string roleCode)
        {
            List<Admin> list = new List<Admin>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GET_GROUPROLE_BY_FILTERS";
                    com.Parameters.Add("GROUP_CODE_IN", string.IsNullOrEmpty(groupCode) ? (object)DBNull.Value : (object)groupCode);
                    com.Parameters.Add("ROLE_CODE_IN", string.IsNullOrEmpty(roleCode) ? (object)DBNull.Value : (object)roleCode);
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                            list.Add(FillShortGroupRoleRecord(dr));
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        private Admin FillShortGroupRoleRecord(OracleDataReader dr)
        {
            Admin groupRole = new Admin();
            var ord = 0;
            if (dr.TryGetOrdinal("GROUP_CODE", out ord) && !dr.IsDBNull(ord))
                groupRole.GroupCode = dr.GetString(ord);
            if (dr.TryGetOrdinal("ROLE_CODE", out ord) && !dr.IsDBNull(ord))
                groupRole.RoleCode = dr.GetString(ord);

            return groupRole;
        }

        public List<string> GetDdlGroupCode_GroupRole()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_GROUPCODE_GROUPROLE";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["GROUP_CODE"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public List<string> GetDdlRoleCode_GroupRole()
        {
            List<string> list = new List<string>();
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandText = "PKG_ADMIN.SP_ROLECODE_GROUPROLE";
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.Parameters.Add("REF_CURSOR", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    using (OracleDataReader dr = com.ExecuteReader())
                        while (dr.Read())
                        {
                            list.Add(dr["ROLE_CODE"].ToString());
                        }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return list;
        }

        public bool InsertGroupRoleInfo(Admin groupRole)
        {
            bool success = false;
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "PKG_ADMIN.SP_GROUPROLE_INSERT";
                    com.Parameters.Add("GROUP_CODE_IN", groupRole.GroupCode);
                    com.Parameters.Add("ROLE_CODE_IN", groupRole.RoleCode);
                    com.Parameters.Add("MAKE_BY_IN", groupRole.MakeBy);
                    com.Parameters.Add("MAKE_DATE_IN", groupRole.MakeDate.ToString("dd MMM yyyy"));

                    com.ExecuteNonQuery();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
            }
            return success;
        }


        public bool FlagDeleteGroupRole(List<Admin> subCodes)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(WebConfig.UcibConnectionString))
                using (OracleCommand com = con.CreateCommand())
                {
                    con.Open();
                    using (OracleTransaction tran = con.BeginTransaction())
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.CommandText = "PKG_ADMIN.SP_GROUPROLE_DELETE";
                        com.Parameters.Add("GROUP_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("ROLE_CODE_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_BY_IN", OracleDbType.Varchar2);
                        com.Parameters.Add("MAKE_DATE_IN", OracleDbType.Date);
                        try
                        {
                            foreach (var subCode in subCodes)
                            {
                                com.Parameters["GROUP_CODE_IN"].Value = subCode.GroupCode;
                                com.Parameters["ROLE_CODE_IN"].Value = subCode.RoleCode;
                                com.Parameters["MAKE_BY_IN"].Value = subCode.MakeBy;
                                com.Parameters["MAKE_DATE_IN"].Value = subCode.MakeDate;
                                com.ExecuteNonQuery();
                            }
                            tran.Commit();
                        }
                        catch
                        {
                            try { tran.Rollback(); }
                            catch { throw; }
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        #endregion


    }

        

}