using HRIS.WebUI.DTOs.Authentication;
using HRIS.WebUI.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HRIS.WebUI.Helpers
{
    public class LoggedInUserController : Controller
    {
        public LoginResponse GetLoggedInUserDetails()
        {
            var response = HttpContext.Session.GetComplexData<LoginResponse>("loggedInUserDetails");
            return response;
        }
    }
}
