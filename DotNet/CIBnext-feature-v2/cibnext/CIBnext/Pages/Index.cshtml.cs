using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CIBnext.Pages
{
    [Authorize(Roles = "ADMIN")]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}