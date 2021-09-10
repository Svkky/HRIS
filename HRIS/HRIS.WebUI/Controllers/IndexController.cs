using HRIS.Application.DTOs.Sales;
using HRIS.Application.Wrappers;
using HRIS.WebUI.Constants;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Threading.Tasks;

namespace HRIS.WebUI.Controllers
{
    [AllowAnonymous]
    public class IndexController : Controller
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public IndexController(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }

        public async Task<IActionResult> Receipts(string billNo)
        {

            var jwtToken = HttpContext.Session.GetString("jwtToken");
            var result = await GetSales(billNo, jwtToken);
            if (result.Succeeded)
            {
                return new ViewAsPdf(result.Data)
                {
                    //FileName = billNo + ".pdf",
                    PageSize = Size.Dle,
                    PageOrientation = Orientation.Portrait,
                    PageMargins = new Margins(0, 0, 0, 0),
                    PageWidth = 70,
                    PageHeight = 150
                };
            }

            return RedirectToPage("/Auth/Login");
        }

        private async Task<Response<SalesVm>> GetSales(string billNumber, string authToken)
        {
            try
            {
                var response = await _httpClientService.GetAsync<Response<SalesVm>>($"{_settings.BaseUrl}{ApplicationContants.GetSaleByBill}{billNumber}", authToken);
                if (response.Succeeded)
                {
                    return response;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
