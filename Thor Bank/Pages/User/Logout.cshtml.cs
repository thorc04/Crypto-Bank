using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Thor_Bank.Pages.User
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            // LOGOUT
            HttpContext.Session.Clear();
            return Redirect("/Index");
        }
    }
}