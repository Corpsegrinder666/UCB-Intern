using CIBnext.DAL;
using CIBnext.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CIBnext.BLL
{
    public class AdminBLL
    {
        public AdminBLL()
        {
        }

        private AdminDAL adminDAL = new AdminDAL();

        #region User Info Page

        public List<DAO.Admin> GetFilteredUserList(string empId, string empName, string branchCode)
        {
            return adminDAL.SelectUsersByFilters(empId, empName, branchCode);
        }

        public bool SaveUserInfo(DAO.Admin user)
        {
            return adminDAL.InsertUserInfo(user);
        }

        public bool FlagDeleteUser(List<DAO.Admin> subCodes)
        {
            return adminDAL.FlagDeleteUser(subCodes);
        }

        #endregion


        #region Roles Page

        public List<Admin> GetFilteredRoleList(string roleCode, string roleDesc)
        {
            return adminDAL.SelectRolesByFilters(roleCode, roleDesc);
        }

        public bool SaveRoleInfo(Admin role)
        {
            return adminDAL.InsertRoleInfo(role);
        }

        public bool FlagDeleteRole(List<Admin> subCodes)
        {
            return adminDAL.FlagDeleteRole(subCodes);
        }

        #endregion


        #region Groups Page

        public List<Admin> GetFilteredGroupList(string GroupCode, string GroupDesc)
        {
            return adminDAL.SelectGroupsByFilters(GroupCode, GroupDesc);
        }

        public bool SaveGroupInfo(Admin Group)
        {
            return adminDAL.InsertGroupInfo(Group);
        }

        public bool FlagDeleteGroup(List<Admin> subCodes)
        {
            return adminDAL.FlagDeleteGroup(subCodes);
        }

        #endregion


        #region User Role Page

        public List<Admin> GetFilteredUserRoleList(string empId, string roleCode)
        {
            return adminDAL.SelectUserRoleByFilters(empId, roleCode);
        }

        public List<string> GetDdlEmpId_UserRole()
        {
            return adminDAL.GetDdlEmpId_UserRole();
        }

        public List<string> GetDdlRoleCode_UserRole()
        {
            return adminDAL.GetDdlRoleCode_UserRole();
        }

        public bool SaveUserRoleInfo(Admin userRole)
        {
            return adminDAL.InsertUserRoleInfo(userRole);
        }

        public bool FlagDeleteUserRole(List<Admin> subCodes)
        {
            return adminDAL.FlagDeleteUserRole(subCodes);
        }

        #endregion


        #region User Group Page

        public List<Admin> GetFilteredUserGroupList(string empId, string groupCode)
        {
            return adminDAL.SelectUserGroupByFilters(empId, groupCode);
        }

        public List<string> GetDdlEmpId_UserGroup()
        {
            return adminDAL.GetDdlEmpId_UserGroup();
        }

        public List<string> GetDdlGroupCode_UserGroup()
        {
            return adminDAL.GetDdlGroupCode_UserGroup();
        }

        public bool SaveUserGroupInfo(Admin userGroup)
        {
            return adminDAL.InsertUserGroupInfo(userGroup);
        }

        public bool FlagDeleteUserGroup(List<Admin> subCodes)
        {
            return adminDAL.FlagDeleteUserGroup(subCodes);
        }

        #endregion


        #region Group Role Page

        public List<Admin> GetFilteredGroupRoleList(string groupCode, string roleCode)
        {
            return adminDAL.SelectGroupRoleByFilters(groupCode, roleCode);
        }

        public List<string> GetDdlGroupCode_GroupRole()
        {
            return adminDAL.GetDdlGroupCode_GroupRole();
        }

        public List<string> GetDdlRoleCode_GroupRole()
        {
            return adminDAL.GetDdlRoleCode_GroupRole();
        }

        public bool SaveGroupRoleInfo(Admin groupRole)
        {
            return adminDAL.InsertGroupRoleInfo(groupRole);
        }

        public bool FlagDeleteGroupRole(List<Admin> subCodes)
        {
            return adminDAL.FlagDeleteGroupRole(subCodes);
        }

        #endregion

    }
}
