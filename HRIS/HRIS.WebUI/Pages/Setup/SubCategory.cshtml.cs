using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Setup
{
    public class SubCategoryModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public SubCategoryModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public IEnumerable<CategoryDto> Categoryies { get; set; }
        public int SubCategoryId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        [RegularExpression(@"^[A-Za-z.-]*$", ErrorMessage = "Description can only take alphabeth")]
        public string Description { get; set; }
        public async Task OnGet()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var branchId = HttpContext.Session.GetString("branchId");

            var response = await _httpClientService.GetAsync<Base<CategoryDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllCategories}", token, branchId);
            Categoryies = response.data;
        }


    }
}
