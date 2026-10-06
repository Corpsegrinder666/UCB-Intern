using CIBnext.LIB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIBnext.Pages
{
    [Authorize(Roles = "ADMIN")]
    public class HomeModel : PageModel
    {
        public string UserName { get; set; }

        public string BranchCode { get; set; }
        public bool IsAdmin { get; set; }
        public IActionResult OnGet()
        {
            var userInfo = AppSession.GetUserInfo(User);

            UserName = userInfo.FullName;
            BranchCode = userInfo.BranchName;

            User.IsInRole("ADMIN");

            return Page();
        }
    }
}