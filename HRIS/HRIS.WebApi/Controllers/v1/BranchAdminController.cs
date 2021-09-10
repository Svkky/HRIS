using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.BranchAdmin;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class BranchAdminController : BaseApiController
    {
        private readonly IBranchAdminService _BranchAdminService;

        public BranchAdminController(IBranchAdminService BranchAdminService)
        {
            _BranchAdminService = BranchAdminService;
        }
        [HttpPost]
        [Route("CreateBranchAdmin")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> CreateBranchAdmin([FromBody] CreateBranchAdminDTO request)
        {
            var response = await _BranchAdminService.CreateBranchAdmin(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating BranchAdmin"));
            return Ok(ResponseHelper.AlreadyExistMessage("You are already registered with us"));
        }
        [HttpPost]
        [Route("UpdateBranchAdmin")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateBranchAdmin([FromBody] UpdateBranchAdminDTO request)
        {
            var response = _BranchAdminService.UpdateBranchAdmin(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating BranchAdmin"));
        }

        [HttpPost]
        [Route("Enable")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Enable([FromBody] DeleteBranchAdminDTO request)
        {
            var response = _BranchAdminService.Enable(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting BranchAdmin"));
        }

        [HttpPost]
        [Route("Disable")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Disable([FromBody] DeleteBranchAdminDTO request)
        {
            var response = _BranchAdminService.Disable(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting BranchAdmin"));
        }

        [HttpPost]
        [Route("DeleteBranchAdmin")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteBranchAdmin([FromBody] DeleteBranchAdminDTO request)
        {
            var response = _BranchAdminService.DeleteBranchAdmin(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("BranchAdmin was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting BranchAdmin"));
        }

        [HttpGet]
        [Route("GetAllBranchAdmin")]
        [ProducesResponseType(typeof(Response<List<BranchAdminDTO>>), 200)]
        public IActionResult GetAllBranchAdmin(DataSourceLoadOptions loadOptions)
        {
            var branchId = Request.Headers["branchId"].ToString();
            var response = _BranchAdminService.GetAllBranchAdmin(int.Parse(branchId));
            var responses = new Response<List<BranchAdminDTO>>();
            var supplierList = new List<BranchAdminDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Supplier list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"branchAdminId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive customer";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
        }


        [HttpGet("customerId")]
        [ProducesResponseType(typeof(Response<BranchAdminDTO>), 200)]
        public IActionResult GetById(int customerId)
        {
            var response = _BranchAdminService.GetBranchAdminById(customerId);
            if (response != null)
                return Ok(new Response<BranchAdminDTO>
                {
                    Data = response,
                    Message = "BranchAdmin was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving BranchAdmin"));
        }

        [HttpGet("GetBranchAdminByPhone/{phone}")]
        [ProducesResponseType(typeof(Response<BranchAdminDTO>), 200)]
        public IActionResult GetBranchAdminByPhone(string phone)
        {
            var response = _BranchAdminService.getBranchAdminByPhone(phone);
            if (response != null)
                return Ok(new Response<BranchAdminDTO>
                {
                    Data = response,
                    Message = "BranchAdmin was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });

            return Ok(ResponseHelper.FailureMessage("Failure retrieving BranchAdmin"));
        }

    }
}
