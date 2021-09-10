using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.CustomerVoucher;
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
    public class CustomerVoucherController : BaseApiController
    {
        private readonly ICustomerVoucherService _customerVoucherService;

        public CustomerVoucherController(ICustomerVoucherService CustomerVoucherService)
        {
            _customerVoucherService = CustomerVoucherService;
        }

        [HttpPost]
        [Route("CreateCustomerVoucher")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateCustomerVoucher([FromBody] CreateCustomerVoucherDTO request)
        {
            var response = _customerVoucherService.CreateCustomerVoucher(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Customer Voucher was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating CustomerVoucher"));
            return Ok(ResponseHelper.AlreadyExistMessage("Customer has been previously assigned the voucher"));
        }

        [HttpPost]
        [Route("RemoveCustomerVoucher")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveCustomerVoucher([FromBody] DeleteCustomerVoucherDTO request)
        {
            var response = _customerVoucherService.DeleteCustomerVoucher(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Customer Voucher was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting CustomerVoucher"));
        }

        [HttpGet]
        [Route("GetAllCustomerVoucher")]
        [ProducesResponseType(typeof(Response<List<CustomerVoucherDTO>>), 200)]
        public IActionResult GetAllCustomerVoucher(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _customerVoucherService.GetAllCustomerVoucher(branchId);
            var responses = new Response<List<CustomerVoucherDTO>>();
           

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Customer Voucher list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"customerVoucherId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FullName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            responses.Data = response;
            responses.Message = "Failure to retrive Customer Voucher";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;

            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FullName), loadOptions));
        }

        [HttpGet("CustomerVoucherId")]
        [ProducesResponseType(typeof(Response<List<CustomerVoucherDTO>>), 200)]
        public IActionResult GetById(int CustomerVoucherId)
        {
            var branchId = int.Parse(Request.Headers["bId"].ToString());
            var response = _customerVoucherService.GetCustomerVoucherById(CustomerVoucherId,branchId);
            if (response != null)
                return Ok(new Response<CustomerVoucherDTO>
                {
                    Data = response,
                    Message = "CustomerVoucher was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving CustomerVoucher"));
        }
    }
}
