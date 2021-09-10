using HRIS.Application.DTOs.Branch;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebUI.Pages.Reports
{
    public class SalesStoreAdminModel : PageModel
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public SalesStoreAdminModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public int BranchId { get; set; }
        public IEnumerable<BranchDTO> BranchDTOs { get; set; }
        public async Task OnGet()
        {
            BranchDTOs = await GetAllBranches();
        }
        public async Task<IEnumerable<BranchDTO>> GetAllBranches()
        {
            var token = HttpContext.Session.GetString("jwtToken");
            var response = await _httpClientService.GetAsync<Base<BranchDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllBranches}", token);
            return response.data;
        }
    }
}
