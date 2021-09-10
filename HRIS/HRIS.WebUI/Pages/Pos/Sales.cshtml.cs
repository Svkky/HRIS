using HRIS.Application.DTOs.Products;
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

namespace HRIS.WebUI.Pages.Pos
{
    public class SalesModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public SalesModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public IEnumerable<BranchProductDTO> ProductList;
        public IEnumerable<ProductTypeVariationDto> ProductTypeList;
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
        public double UnitPrice { get; set; }
        public double SaleQuantity { get; set; }
        public double Vat { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerFullName { get; set; }
        public string DiscountVoucher { get; set; }
        public double TotalAmount { get; set; }
        public string ModeOfPayment { get; set; }
        public double AmountPaid { get; set; }
        public async Task OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwtToken");
            var branchId = HttpContext.Session.GetString("branchId");
            ProductList = await GetProducts(jwtToken, branchId);
            // ProductTypeList = await GetProductType(jwtToken, branchId);
        }
        public async Task<IEnumerable<BranchProductDTO>> GetProducts(string token, string branchId)
        {
            var result = await _httpClientService.GetAsync<Base<BranchProductDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetBranchProducts}", token, branchId);

            return result.data;
        }
        //public async Task<IEnumerable<ProductTypeVariationDto>> GetProductType(string token, string branchId)
        //{
        //    var result = await _httpClientService.GetAsync<Base<ProductTypeVariationDto>>($"{_settings.BaseUrl}{ApplicationContants.GetProductType}", token, branchId);

        //    return result.data;
        //}
    }
}
