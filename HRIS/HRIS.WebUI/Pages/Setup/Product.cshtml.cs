using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.DTOs.Product;
using HRIS.WebUI.DTOs.Setup;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Setup
{
    public class ProductModel : PageModel
    {
        public IEnumerable<ProductTypeVariationDto> ProductTypeList;
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public double Vat { get; set; }
        public decimal QuantityRemaining { get; set; }
        public decimal SellPrice { get; set; }
        public string CanExpire { get; set; }
        public string IsDiscount { get; set; }
        public decimal Discount { get; set; }
        public string IsProductType { get; set; }
        public int VariantNameId { get; set; }
        public string VariantQty { get; set; }
        public int CriticalLevel { get; set; }
        public string IsVat { get; set; }
        public int BranchId { get; set; }


        public ProductModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public async Task<IActionResult> OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwtToken");
            var branchId = HttpContext.Session.GetString("branchId");
            BranchId = int.Parse(branchId);
            ProductTypeList = await GetProductType(jwtToken, branchId);

            return Page();

        }


        //public async Task<IEnumerable<CategoryDto>> GetCategory(string token, string branchId)
        //{
        //    var customerResponse = await _httpClientService.GetAsync<Base<CategoryDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllCategories}", token, branchId);

        //    return customerResponse.data;
        //}
        public async Task<IEnumerable<ProductAllocationDTO>> GetProducts(string token, string branchId)
        {
            var customerResponse = await _httpClientService.GetAsync<Base<ProductAllocationDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetProductsAllocatedToBranch}", token, branchId);

            return customerResponse.data;
        }

        //public async Task<IEnumerable<SupplyDto>> GetVendor(string token, string branchId)
        //{
        //    var customerResponse = await _httpClientService.GetAsync<Base<SupplyDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllSupllier}", token, branchId);

        //    return customerResponse.data;
        //}


        public async Task<IEnumerable<ProductTypeVariationDto>> GetProductType(string token, string branchId)
        {
            var customerResponse = await _httpClientService.GetAsync<Base<ProductTypeVariationDto>>($"{_settings.BaseUrl}{ApplicationContants.GetProductType}", token, branchId);

            return customerResponse.data;
        }


    }
}
