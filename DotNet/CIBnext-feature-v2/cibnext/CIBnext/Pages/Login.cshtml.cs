using CIBnext.DAO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIBnext.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userMgr;
        private readonly SignInManager<ApplicationUser> _signInMgr;

        public LoginModel(
            UserManager<ApplicationUser> userMgr,
            SignInManager<ApplicationUser> signInMgr)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
        }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (string.IsNullOrWhiteSpace(UserName))
            {
                ErrorMessage = "Enter User Name";
                return Page();
            }

            var user = await _userMgr.FindByNameAsync(UserName);

            if (user == null)
            {
                ErrorMessage = "User Not Found";
                return Page();
            }

            if (Password != "123")
            {
                ErrorMessage = "Invalid Password";
                return Page();
            }

            await _signInMgr.SignInAsync(user, false);

            HttpContext.Session.SetString(
                "LoginTime",
                DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"));

            return RedirectToPage("/Home");
        }
    }
}