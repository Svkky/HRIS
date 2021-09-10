using HRIS.Application.DTOs.StoreProduct;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.DTOs.Setup;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Payment
{
    public class VendorPaymentModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public VendorPaymentModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public int SupplierId { get; set; }
        public int TotalCarton { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalItemPerPack { get; set; }
        public int TotalAmount { get; set; }
        public int AmountPaid { get; set; }
        public int Balance { get; set; }
        public int DelieveryDate { get; set; }
        public IEnumerable<StoreProductDTO> StoreProductDTO { get; set; }
        public IEnumerable<SupplyDto> Suppliers { get; set; }
        public async Task OnGet()
        {
            StoreProductDTO = await GetProducts();
            Suppliers = await GetVendor();
        }
        public async Task<IEnumerable<StoreProductDTO>> GetProducts()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<StoreProductDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllStoreProduct}", token);
            return response.data;
        }
        public async Task<IEnumerable<SupplyDto>> GetVendor()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<SupplyDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllSupllier}", token);

            return response.data;
        }
    }
}
