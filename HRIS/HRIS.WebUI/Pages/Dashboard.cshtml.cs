using HRIS.Application.DTOs.Dashboard;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages
{
    public class DashboardModel : PageModel
    {


        public List<DashBoardDTO> UserList;
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;

        // [Required]
        public string TotalPaid { get; set; }
        // [Required]
        public string TotalVat { get; set; }
        //[Required]
        public string Orders { get; set; }

        // [Required]
        public int BranchID { get; set; }

        public DashboardModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["PageName"] = "Dashboard";
            var jwtToken = HttpContext.Session.GetString("jwtToken");
            UserList = await GetList(jwtToken);
            //TotalPaid = UserList;
            //Orders = UserList.Orders;
            //TotalVat = UserList.TotalVat;
            foreach (var item in UserList)
            {

                TotalPaid = decimal.Parse(item.TotalPaid).ToString("n2");
                Orders = item.Orders;
                TotalVat = decimal.Parse(item.TotalVat).ToString("n2");
            }
            return Page();
        }

        public async Task<List<DashBoardDTO>> GetList(string token)
        {
            var customerResponse = await _httpClientService.GetAsync<Base<DashBoardDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllDashboardValues}", token);


            var polly = new List<DashBoardDTO>();
            customerResponse.data.ForEach(x =>
            {
                polly.Add(new DashBoardDTO
                {
                    Orders = x.Orders,
                    TotalVat = x.TotalVat,
                    TotalPaid = x.TotalPaid

                });
            });
            //revenueMappings.revenues = RevenueItems;
            UserList = polly as List<DashBoardDTO>;
            return UserList;

        }




    }

}
