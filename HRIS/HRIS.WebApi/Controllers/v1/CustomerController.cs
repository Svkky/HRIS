using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Customer;
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
    public class CustomerController : BaseApiController
    {
        private readonly ICustomerService _CustomerService;

        public CustomerController(ICustomerService CustomerService)
        {
            _CustomerService = CustomerService;
        }
        [HttpPost]
        [Route("CreateCustomer")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateCustomer([FromBody] CreateCustomerDTO request)
        {
            var response = _CustomerService.CreateCustomer(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Customer was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating Customer"));
            return Ok(ResponseHelper.AlreadyExistMessage("You are already registered with us"));
        }
        [HttpPost]
        [Route("UpdateCustomer")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateCustomer([FromBody] UpdateCustomerDTO request)
        {
            var response = _CustomerService.UpdateCustomer(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Customer was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Customer"));
        }

        [HttpPost]
        [Route("DeleteCustomer")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteCustomer([FromBody] DeleteCustomerDTO request)
        {
            var response = _CustomerService.DeleteCustomer(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Customer was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Customer"));
        }

        [HttpGet]
        [Route("GetAllCustomer")]
        [ProducesResponseType(typeof(Response<List<CustomerDTO>>), 200)]
        public IActionResult GetAllCustomer(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _CustomerService.GetAllCustomer(branchId);
            var responses = new Response<List<CustomerDTO>>();
            var supplierList = new List<CustomerDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Supplier list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"customerId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FullName), loadOptions));
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
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FullName), loadOptions));
        }


        [HttpGet("customerId")]
        [ProducesResponseType(typeof(Response<CustomerDTO>), 200)]
        public IActionResult GetById(int customerId)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _CustomerService.GetCustomerById(customerId, branchId);
            if (response != null)
                return Ok(new Response<CustomerDTO>
                {
                    Data = response,
                    Message = "Customer was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Customer"));
        }

        [HttpGet("GetCustomerByPhone/{phone}")]
        [ProducesResponseType(typeof(Response<CustomerDTO>), 200)]
        public IActionResult GetCustomerByPhone(string phone)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _CustomerService.getCustomerByPhone(phone, branchId);
            if (response != null)
                return Ok(new Response<CustomerDTO>
                {
                    Data = response,
                    Message = "Customer was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });

            return Ok(ResponseHelper.FailureMessage("Failure retrieving Customer"));
        }

    }
}
