using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Account;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Wrappers;
using HRIS.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, IAuthenticatedUserService authenticatedUserService, IJwtService jwtService)
        {
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
            _jwtService = jwtService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GenerateIPAddress()));
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.RegisterAsync(request, origin));
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code, [FromQuery] string password)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.ConfirmEmailAsync(userId, code, password));
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        {
            await _accountService.ForgotPassword(model, Request.Headers["origin"]);
            return Ok();
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {

            return Ok(await _accountService.ResetPassword(model));
        }
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            return Ok(await _accountService.Logout());
        }
        [HttpGet("get-users")]
        [Authorize]
        public async Task<IActionResult> GetUsers(DataSourceLoadOptions loadOptions)
        {
            var response = await _accountService.GetUsers(_authenticatedUserService.UserId);
            var responses = new Response<List<UserDTO>>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "User list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"userId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive user";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
        }
        [HttpPost("DeleteUser/{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            return Ok(await _accountService.RemoveUser(userId));
        }
        [HttpGet("get-roles")]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _accountService.GetRoles());
        }
        [HttpGet("ResendEmail/{email}")]
        [Authorize]
        public async Task<IActionResult> ResendEmail(string email)
        {
            var webUrl = HttpContext.Request.Headers["webUrl"].ToString();
            return Ok(await _accountService.ResendEmail(email, webUrl));
        }
        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpPost]
        [Route("Enable/{userId}")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Enable(string userId)
        {
            var response = await _accountService.Enable(userId);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting BranchAdmin"));
        }

        [HttpPost]
        [Route("Disable/{userId}")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Disable(string userId)
        {
            var response = await _accountService.Disable(userId);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting BranchAdmin"));
        }
        [HttpPost]
        [Route("hasexpired/{token}")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult HasTokenExpired(string token)
        {
            var response = _jwtService.GetExpiryTimestamp(token);
            if (DateTime.Now >= response)
            {
                return Ok(ResponseHelper.FailureMessage("token expired"));
            }
            else
            {
                return Ok(ResponseHelper.SuccessMessage("token still active"));
            }

        }


    }


}