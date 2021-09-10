using HRIS.Application.Wrappers;
using HRIS.WebUI.Constants;
using HRIS.WebUI.DTOs.Authentication;
using HRIS.WebUI.DTOs.Setup;
using HRIS.WebUI.Extensions;
using HRIS.WebUI.Helpers;
using HRIS.WebUI.Interfaces;
using HRIS.WebUI.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace HRIS.WebUI.Services
{
    public class MenuSetup : IMenuSetup
    {
        private readonly IHttpClientService _httpClientService;
        private readonly ApiConfiguration _settings;
        public MenuSetup(IHttpClientService httpClientService, IOptions<ApiConfiguration> settings)
        {
            _httpClientService = httpClientService;
            _settings = settings.Value;
        }
        public string Greetingmsgs()
        {
            var currentdate = DateTime.Now.ToString("hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            var createdate1 = DateTime.Now.ToString("hh:mm:ss.fff tt", System.Globalization.DateTimeFormatInfo.InvariantInfo);

            var currenttimeformat = Convert.ToDateTime(currentdate);

            var finaltime = Math.Round((decimal)currenttimeformat.Hour);
            string GreetMessage;
            if (finaltime >= 5 && finaltime <= 11)
            {
                //var logUserName = Convert.ToString(TempData["FirstName"]).ToUpper() + "!";
                //GreetMessage = "Good Morning!" + "<br/>" + "Have a nice day";
                GreetMessage = "Good Morning! Have a nice day.";

            }
            else if (finaltime == 12)
            {
                GreetMessage = "Good Afternoon";
            }
            else if (finaltime >= 13 && finaltime <= 17)
            {
                GreetMessage = "Good Afternoon";
            }
            else if (finaltime >= 18 && finaltime <= 20)
            {
                GreetMessage = "Good Evening";
            }
            else if (finaltime >= 21 && finaltime <= 11)
            {
                //GreetMessage = "Good Night !" + "<br/>" + "Have a nice night";
                GreetMessage = "Good Evening";
            }
            else
            {
                //GreetMessage = "Wow !" + "<br/>" + "You`re still awake. Working Late?";
                GreetMessage = "Wow! You`re still awake. Working Late?";
            }

            return GreetMessage;
        }

        public LoginResponse GetLoggedInUserDetails()
        {
            var _loggedInUser = new LoggedInUserController();
            var user = _loggedInUser.GetLoggedInUserDetails();
            return user;
        }
        public List<MenuItemSetup> GetMenuSetupById(string userId)
        {
            var response = _httpClientService.GetAsync<Response<List<MenuItemSetup>>>($"{_settings.BaseUrl}{ApplicationContants.GetMenuSetup}{userId}", "").Result;
            if (response == null)
            {
                return new List<MenuItemSetup>();
            }
            else
            {
                if (response.Succeeded)
                {
                    return response.Data;
                }
                return new List<MenuItemSetup>();
            }
        }
        public string GetStoreName()
        {
            var response = _httpClientService.GetAsync<Response<string>>($"{_settings.BaseUrl}{ApplicationContants.GetStoreName}", "").Result;
            if (response == null)
            {
                return "";
            }
            else
            {
                if (response.Succeeded)
                {
                    return response.Data;
                }
                return "";
            }
        }
    }
}
