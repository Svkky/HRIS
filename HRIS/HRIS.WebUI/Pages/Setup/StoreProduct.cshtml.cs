using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Setup
{
    public class StoreProductModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public StoreProductModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public IEnumerable<CategoryDto> Categories { get; set; }
        public IEnumerable<SubCategoryDto> SubCategories { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string ProductName { get; set; }
        public async Task OnGet()
        {
            Categories = await GetCategories();
            SubCategories = await GetSubCategories();
        }
        public async Task<List<CategoryDto>> GetCategories()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            //var branchId = HttpContext.Session.GetString("branchId");
            var response = await _httpClientService.GetAsync<Base<CategoryDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllCategories}", token);
            return response.data;
        }
        public async Task<List<SubCategoryDto>> GetSubCategories()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<SubCategoryDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllSubCategories}", token);
            return response.data;
        }
    }
}
