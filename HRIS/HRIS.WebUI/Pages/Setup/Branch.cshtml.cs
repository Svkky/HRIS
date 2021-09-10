using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Branch;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Category;
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
    public class BranchModel : PageModel
    {
        public IEnumerable<MyUsersVM> UserList;
        public IEnumerable<MyUsersVM> BranchList;
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;

        [Required]
        public string UserID { get; set; }

        [Required]
        public int BranchID { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression("^[+0-9]*$", ErrorMessage = "Input a valid Phone Number.")]
        [MaxLength(15, ErrorMessage = "Lenght Exceeded..")]
        [MinLength(11, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public BranchModel(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }

        public async Task<IActionResult> OnGet()
        {
            var jwtToken = HttpContext.Session.GetString("jwtToken");

            UserList = await GetUsers(jwtToken);
            BranchList = await GetBranch(jwtToken);

            return Page();
        }

        public async Task<IEnumerable<MyUsersVM>> GetUsers(string token)
        {
            var customerResponse = await _httpClientService.GetAsync<Base<UserDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllUsers}", token);


            var polly = new List<MyUsersVM>();
            customerResponse.data.ForEach(x =>
            {
                polly.Add(new MyUsersVM
                {
                    text = x.LastName + ' ' + x.FirstName,
                    value = x.UserId
                });
            });

            return polly;

        }



        public async Task<IEnumerable<MyUsersVM>> GetBranch(string token)
        {
            var voucherResponse = await _httpClientService.GetAsync<Base<BranchDTO>>($"{_settings.BaseUrl}{ApplicationContants.GetAllBranches}", token);

            var polly = new List<MyUsersVM>();
            voucherResponse.data.ForEach(x =>
            {
                polly.Add(new MyUsersVM
                {
                    text = x.branchName,
                    value = x.branchID.ToString()
                });
            });

            return polly;
        }
    }
}
