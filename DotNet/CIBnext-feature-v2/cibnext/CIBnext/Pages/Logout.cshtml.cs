using CIBnext.DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIBnext.Pages
{
    public class LogoutModel (SignInManager<ApplicationUser> signInMgr) : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            await signInMgr.SignOutAsync();
         
            return RedirectToPage("/Login");
        }
    }
}