using HRIS.Application.DTOs.Account;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Base;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Adminsitration
{
    public class UsersModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public UsersModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public string UserId { get; set; }
        [Required]
        [Display(Name = ("Role Name"))]
        public string RoleId { get; set; }
        [Required]
        [Display(Name = ("First Name"))]
        [RegularExpression(@"^[A-Z a-z.-]*$", ErrorMessage = "First name can take alphabets only.")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = ("Last Name"))]
        [RegularExpression(@"^[A-Z a-z.-]*$", ErrorMessage = "Last name can take alphabets only.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression(@"^[+0-9]*$", ErrorMessage = "Please enter a valid Phone number")]
        [MaxLength(15, ErrorMessage = "Please enter a valid Phone number")]
        [MinLength(11, ErrorMessage = "Please enter a valid Phone number")]
        [Phone(ErrorMessage = "Please enter a valid Phone number")]
        public string PhoneNumber { get; set; }

        public List<RolesDTO> Roles { get; set; }
        public async Task<IActionResult> OnGet()
        {
            var siteLocation = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
            HttpContext.Session.SetString("webUrl", siteLocation);
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<BaseDTO<List<RolesDTO>>>($"{_settings.BaseUrl}{ApplicationContants.GetRoles}", token);
            if (response.succeeded)
            {
                Roles = response.data;
            }

            return Page();
        }
    }
}
