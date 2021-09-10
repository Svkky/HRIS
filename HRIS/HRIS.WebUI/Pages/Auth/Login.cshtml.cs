using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Authentication;
using HRIS.WebUI.DTOs.Base;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Auth
{
    [ValidateAntiForgeryToken]
    public class LoginModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public LoginModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [BindProperty]
        public bool RememberMe { get; set; }
        public IActionResult OnGet(string verified)
        {
            var isVerified = HttpContext.Session.GetString("verified");
            if (isVerified != null)
            {
                HttpContext.Session.Remove("verified");
                return Page();
            }
            if (verified != null)
                HttpContext.Session.SetString("verified", "Yes");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var loginRequest = new LoginRequestDTO { email = this.Email, password = this.Password };

            var response = await _httpClientService.PostAsync<LoginRequestDTO, BaseDTO<LoginResponse>>($"{_settings.BaseUrl}{ApplicationContants.Authenticate}", loginRequest);

            if (!response.succeeded)
            {
                ModelState.AddModelError("", response.message);
                return Page();
            }
            //var claims = new List<Claim>();

            //claims.Add(new Claim(ClaimTypes.Name, response.data.userName));
            //var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var claimPrincipal = new ClaimsPrincipal(claimIdenties);
            //var authenticationManager = Request.HttpContext;

            //// Sign In.  
            //await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { });

            // HttpContext.Session.SetComplexData("loggedInUserDetails", response.data);
            HttpContext.Session.SetString("jwtToken", response.data.jwToken);
            HttpContext.Session.SetString("userId", response.data.id);
            HttpContext.Session.SetString("role", response.data.roles.FirstOrDefault());
            HttpContext.Session.SetString("FullName", response.data.fullName);
            HttpContext.Session.SetString("Role", response.data.roles.FirstOrDefault());
            HttpContext.Session.SetString("BaseUrl", _settings.BaseUrl);
            var siteLocation = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            HttpContext.Session.SetString("BaseUrl", _settings.BaseUrl);
            HttpContext.Session.SetString("siteLocation", siteLocation);
            HttpContext.Session.SetString("webUrl", siteLocation);
            if (response.data.branchId != 0)
                HttpContext.Session.SetString("branchId", response.data.branchId.ToString());

            //return new RedirectToPageResult("/Portal/Login");

            return RedirectToPage("/Dashboard");
        }
    }
}
