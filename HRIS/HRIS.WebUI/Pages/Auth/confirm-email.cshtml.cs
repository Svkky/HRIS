using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Base;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Auth
{
    public class confirm_emailModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public confirm_emailModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        [Required]
        [BindProperty]
        public string Password { get; set; }
        [Required]
        [BindProperty]
        public string ConfirmPassword { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string Code { get; set; }
        public IActionResult OnGet(string code, string userId)
        {
            if (code == null || userId == null)
                ViewData["BadRequest"] = "Yes";
            Code = code;
            UserId = userId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _httpClientService.GetAsync<BaseDTO<string>>($"{_settings.BaseUrl}{ApplicationContants.confirm_email}?userId={UserId}&code={Code}&password={Password}");
                if (result.succeeded)
                {
                    return RedirectToPage("/Auth/Login", new { verified = "yes" });
                }
                ModelState.AddModelError("", "verification failed");
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error occured while processign your request");
                return Page();
            }
        }
    }
}
