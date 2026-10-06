using CIBnext.DAO;
using Microsoft.AspNetCore.Identity;

namespace CIBnext.DAO
{
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeId
        {
            get
            {
                return this.Id;
            }
            set
            {
                this.Id = value;
            }
        }

        public AppUser UserInfo { get; set; }
    }
}
