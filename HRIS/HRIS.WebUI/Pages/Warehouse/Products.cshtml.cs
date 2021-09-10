using HRIS.Application.DTOs.StoreProduct;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Warehouse
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public ProductsModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public int ProductId { get; set; }
        public IEnumerable<StoreProductDTO> StoreProducts { get; set; }
        public async Task OnGet()
        {
            StoreProducts = await GetProducts();
        }
        public async Task<IEnumerable<StoreProductDTO>> GetProducts()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<StoreProductDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllStoreProduct}", token);
            return response.data;
        }

    }
}
