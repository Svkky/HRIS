using HRIS.Application.DTOs.Branch;
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

namespace HRIS.WebUI.Pages.Setup
{
    public class ProductAllocationModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public ProductAllocationModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public int AllocationQuantity { get; set; }
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public IEnumerable<StoreProductDTO> Products { get; set; }
        public IEnumerable<BranchDTO> Branches { get; set; }
        public async Task OnGet()
        {
            Products = await GetProducts();
            Branches = await GetBranches();
        }

        public async Task<IEnumerable<StoreProductDTO>> GetProducts()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<StoreProductDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllStoreProduct}", token);
            return response.data;
        }
        public async Task<IEnumerable<BranchDTO>> GetBranches()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<BranchDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllBranches}", token);
            return response.data;
        }
    }
}
