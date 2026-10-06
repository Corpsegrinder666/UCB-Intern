using CIBnext.DAO;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CIBnext.BLL
{
    public class UserStore : IUserStore<ApplicationUser>,
                             IUserClaimStore<ApplicationUser>,
                             IUserRoleStore<ApplicationUser>,
                             IUserSecurityStampStore<ApplicationUser>,
                             IQueryableUserStore<ApplicationUser>
    {
        private readonly AccountBLL _accountBll;
        private readonly MiscBLL _miscBLL;

        public IQueryable<ApplicationUser> Users =>
            throw new NotImplementedException();

        public UserStore(AccountBLL accountBll, MiscBLL miscBLL)
        {
            _accountBll = accountBll;
            this._miscBLL = miscBLL;
        }

        public void Dispose()
        {
        }

        public async Task<ApplicationUser?> FindByNameAsync(
            string normalizedUserName,
            CancellationToken cancellationToken)
        {
            var user = _accountBll.GetUser(normalizedUserName.ToLower());

            if (user == null)
            {
                return null;
            }

            var param = await _miscBLL.GetParameters();
            user.Parameters = param;


            return new ApplicationUser
            {
                UserName = user.DomainID,
                NormalizedUserName = user.DomainID.ToLower(),
                EmployeeId = user.EmployeeID.ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserInfo = user
            };
        }

        public Task<ApplicationUser?> FindByIdAsync(
            string userId,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<ApplicationUser?>(null);
        }

        public Task<string?> GetUserNameAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(
            ApplicationUser user,
            string? userName,
            CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string?> GetNormalizedUserNameAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task SetNormalizedUserNameAsync(
            ApplicationUser user,
            string? normalizedName,
            CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<string> GetUserIdAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmployeeId);
        }

        public Task<string?> GetSecurityStampAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(
            ApplicationUser user,
            string stamp,
            CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public Task<IList<Claim>> GetClaimsAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            IList<Claim> claims = new List<Claim>
            {
               new Claim("ADID", user.UserInfo.DomainID ?? ""),
               new Claim("FullName", user.UserInfo.FullName ?? ""),
                new Claim("BranchCode", user.UserInfo.BranchCode ?? ""),
               new Claim("BranchName", user.UserInfo.BranchName ?? ""),
               new Claim("CibBranchCode", user.UserInfo.CibBranchCode ?? ""),
               new Claim("EmployeeId", user.UserInfo.EmployeeID.ToString()),
               new Claim("ReportingPeriod",
            user.UserInfo.Parameters != null
                ? user.UserInfo.Parameters.ReportingPeriod.ToString("dd-MMM-yyyy")
                : DateTime.Now.ToString("dd-MMM-yyyy"))
            };

            return Task.FromResult(claims);
        }

        public Task<IList<string>> GetRolesAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            IList<string> roles =
                _accountBll.GetUserRolesByAD(user.UserInfo.DomainID).ToList();

            return Task.FromResult(roles);
        }

        public Task<bool> IsInRoleAsync(
            ApplicationUser user,
            string roleName,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(
            string roleName,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<ApplicationUser>>(
                new List<ApplicationUser>());
        }

        public Task AddClaimsAsync(
            ApplicationUser user,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task ReplaceClaimAsync(
            ApplicationUser user,
            Claim claim,
            Claim newClaim,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RemoveClaimsAsync(
            ApplicationUser user,
            IEnumerable<Claim> claims,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<IList<ApplicationUser>> GetUsersForClaimAsync(
            Claim claim,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<IList<ApplicationUser>>(
                new List<ApplicationUser>());
        }

        public Task AddToRoleAsync(
            ApplicationUser user,
            string roleName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task RemoveFromRoleAsync(
            ApplicationUser user,
            string roleName,
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> UpdateAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(
            ApplicationUser user,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(IdentityResult.Success);
        }
    }
}