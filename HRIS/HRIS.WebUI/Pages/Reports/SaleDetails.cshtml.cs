using HRIS.Application.DTOs.Sales;
using HRIS.Application.Wrappers;
using HRIS.WebUI.Constants;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Reports
{
    public class SaleDetailsModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public SaleDetailsModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public string BillNumber { get; set; }
        public string TotalAmount { get; set; }
        public string TotalVat { get; set; }
        public string TotalDiscount { get; set; }
        public string DatePaid { get; set; }

        public async Task OnGet(string billNumber)
        {
            HttpContext.Session.SetString("billNumber", billNumber);
            BillNumber = billNumber;
            var response = await GetSaleByBill(billNumber);
            TotalAmount = response.TotalAmount;
            TotalVat = response.TotalVat;
            TotalDiscount = response.TotalDiscount;
            DatePaid = response.DatePaid;
        }
        public async Task<SalesVm> GetSaleByBill(string billNumber)
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Response<SalesVm>>($"{_settings.BaseUrl}{ApplicationContants.GetSaleByBill}{billNumber}", token);
            return response.Data;
        }
    }
}
