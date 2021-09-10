using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HRIS.WebUI.Pages.Administration
{
    public class ManageUserPriviledgeModel : PageModel
    {
        public IActionResult OnGet()
        {
            var userIdExists = HttpContext.Session.GetString("userIdd");
            var roleIdd = HttpContext.Session.GetString("roleIdd");
            if ((userIdExists == null || userIdExists == "") || (roleIdd == null || roleIdd == ""))
            {
                string userId = HttpContext.Request.Query["userId"].ToString();
                string roleId = HttpContext.Request.Query["roleId"].ToString();
                HttpContext.Session.SetString("roleIdd", roleId);
                HttpContext.Session.SetString("userIdd", userId);
                return RedirectToPage("/Administration/ManageUserPriviledge");
            }
            ViewData["roleId"] = HttpContext.Session.GetString("roleIdd");
            ViewData["userId"] = HttpContext.Session.GetString("userIdd");
            return Page();
        }

    }
}
