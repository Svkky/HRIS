using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.DTOs.Setup;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Setup
{
    public class CustomerVoucherModel : PageModel
    {
        public IEnumerable<CustomerVm> CustomerList;
        public IEnumerable<VoucherVm> VoucherList;
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int VoucherId { get; set; }


        public CustomerVoucherModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }

        public async Task<IActionResult> OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwtToken");
            var branchId = HttpContext.Session.GetString("branchId");

            CustomerList = await GetCustomer(jwtToken, branchId);
            VoucherList = await GetVoucher(jwtToken, branchId);

            return Page();
        }

        public async Task<IEnumerable<CustomerVm>> GetCustomer(string token, string branchId)
        {
            var customerResponse = await _httpClientService.GetAsync<Base<CustomerDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllCustomer}", token, branchId);


            var polly = new List<CustomerVm>();
            customerResponse.data.ForEach(x =>
            {
                polly.Add(new CustomerVm
                {
                    text = x.FullName + ".../" + x.Phone,
                    value = x.CustomerId
                });
            });

            return polly;

        }



        public async Task<IEnumerable<VoucherVm>> GetVoucher(string token, string branchId)
        {
            var voucherResponse = await _httpClientService.GetAsync<Base<VoucherDto>>($"{_settings.BaseUrl}{ApplicationContants.GetAllVoucher}", token, branchId);

            var polly = new List<VoucherVm>();
            voucherResponse.data.ForEach(x =>
            {
                polly.Add(new VoucherVm
                {
                    text = x.description,
                    value = x.voucherId
                });
            });

            return polly;
        }

    }
}
