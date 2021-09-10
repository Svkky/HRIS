using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Supplier;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class SupplierController : BaseApiController
    {
        private readonly ISupplierService _SupplierService;

        public SupplierController(ISupplierService SupplierService)
        {
            _SupplierService = SupplierService;
        }
        [HttpPost]
        [Route("CreateSupplier")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateSupplier([FromBody] CreateSupplierDTO request)
        {
            var response = _SupplierService.CreateSupplier(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Supplier was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating Supplier"));
            return Ok(ResponseHelper.AlreadyExistMessage("You are already registered with us"));
        }
        [HttpPost]
        [Route("UpdateSupplier")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateSupplier([FromBody] UpdateSupplierDTO request)
        {
            var response = _SupplierService.UpdateSupplier(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Supplier was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Supplier"));
        }

        [HttpPost]
        [Route("DeleteSupplier")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteSupplier([FromBody] DeleteSupplierDTO request)
        {
            var response = _SupplierService.DeleteSupplier(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Supplier was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Supplier"));
        }

        [HttpGet]
        [Route("GetAllSupplier")]
        [ProducesResponseType(typeof(Response<List<SupplierDTO>>), 200)]
        public IActionResult GetAllSupplier(DataSourceLoadOptions loadOptions)
        {
            //  var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _SupplierService.GetAllSupplier();
            var responses = new Response<List<SupplierDTO>>();
            var supplierList = new List<SupplierDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Supplier list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"supplierId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Name), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive supplier";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Name), loadOptions));
        }
        [HttpGet("supplierId")]
        [ProducesResponseType(typeof(Response<List<SupplierDTO>>), 200)]
        public IActionResult GetById(int supplierId)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _SupplierService.GetSupplierById(supplierId);
            if (response != null)
                return Ok(new Response<SupplierDTO>
                {
                    Data = response,
                    Message = "Supplier was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Supplier"));
        }
    }
}
