using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Menu;
using HRIS.Application.Wrappers;
using HRIS.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.Role
{
    [ApiVersion("1.0")]
    [Authorize]
    public class MenuItemController : BaseApiController
    {
        private readonly IMenuRepository _menuRepository;

        public MenuItemController(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;

        }
        [HttpGet("GetPagesByRoleId/{roleId}")]
        [ProducesResponseType(typeof(Response<List<MenuRole>>), 200)]
        public async Task<IActionResult> GetPagesByRoleId(string roleId)
        {
            var response = await _menuRepository.GetMenu(roleId);
            if (response != null)
                return Ok(new Response<List<MenuRole>>
                {
                    Data = response,
                    Message = "Menulist  was retrieved successfully ",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Menulist"));
        }
        [AllowAnonymous]
        [HttpGet("GetPagesAssignedToUser/{userId}")]
        [ProducesResponseType(typeof(Response<List<MenuRole>>), 200)]
        public async Task<IActionResult> GetPagesAssignedToUser(string userId)
        {
            var response = await _menuRepository.GetPagesAssignedToUser(userId);
            if (response != null)
                return Ok(new Response<List<MenuRole>>
                {
                    Data = response,
                    Message = "Menulist  was retrieved successfully ",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Menulist"));
        }
        [AllowAnonymous]
        [HttpGet("GetStoreName")]
        [ProducesResponseType(typeof(Response<List<MenuRole>>), 200)]
        public async Task<IActionResult> GetStoreName()
        {
            var response = await _menuRepository.GetStoreName();
            if (response != null)
                return Ok(new Response<string>
                {
                    Data = response,
                    Message = "storeName  was retrieved successfully ",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving storeName"));
        }


        [HttpGet("GetPagesByUserId/{userId}")]
        [ProducesResponseType(typeof(Response<List<PermissionDTO>>), 200)]
        public IActionResult GetPagesByUserId(string userId)
        {
            var response = _menuRepository.GetPermissionbyUserID(userId);
            if (response != null)
                return Ok(new Response<List<PermissionDTO>>
                {
                    Data = response,
                    Message = "Permission was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Permission"));
        }
        [AllowAnonymous]
        [HttpPost("CreateMenu")]
        [ProducesResponseType(typeof(Response<List<PermissionDTO>>), 200)]
        public IActionResult CreateMenu([FromBody] CreateMenuDTO request)
        {
            var response = _menuRepository.CreateMenuSetup(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Menu was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Menu creating category"));
            return Ok(ResponseHelper.AlreadyExistMessage("Menu exists"));
        }

    }
}
