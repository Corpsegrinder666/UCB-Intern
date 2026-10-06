using CIBnext.DAL;
using CIBnext.DAO;

namespace CIBnext.BLL
{
    public class AccountBLL
    {
        private readonly AccountDAL _accDal;

        public AccountBLL(AccountDAL accDal)
        {
            _accDal = accDal;
        }

        public AppUser? GetUser(string username)
        {
            return _accDal.SelectUserByAd(username);
        }

        public string[] GetUserRolesByAD(string username)
        {
            return _accDal.SelectUserRolesByAd(username);
        }
    }
}