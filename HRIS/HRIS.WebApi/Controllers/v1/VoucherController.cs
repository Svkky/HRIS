using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Voucher;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService service;
        public VoucherController(IVoucherService service)
        {
            this.service = service;
        }

        [HttpPost("CreateVoucher")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateVoucher([FromBody] CreateVoucherDTO request)
        {
            var response = service.CreateVoucher(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Voucher was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating Voucher"));
        }

        [HttpPost("UpdateVoucher")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateVoucher([FromBody] UpdateVoucherDTO request)
        {
            var response = service.UpdateVoucher(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Voucher was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Voucher"));
        }

        [HttpPost("RemoveVoucher")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveVoucher([FromBody] DeleteVoucherDTO request)
        {
            var response = service.DeleteVoucher(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Voucher was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Voucher"));
        }

        [HttpGet("GetAllVoucher")]
        [ProducesResponseType(typeof(Response<List<VoucherDTO>>), 200)]
        public IActionResult GetAllVoucher(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = service.GetAllVoucher(branchId);
            var responses = new Response<List<VoucherDTO>>();

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Voucher list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"voucherId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.description), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            responses.Data = response;
            responses.Message = "Failure to retrive voucher";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.description), loadOptions));
        }

        [HttpGet("GetVoucher")]
        [ProducesResponseType(typeof(Response<VoucherDTO>), 200)]
        public IActionResult Get([FromHeader] int VoucherId)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = service.GetVoucherById(VoucherId,branchId);
            if (response != null)
                return Ok(new Response<VoucherDTO>
                {
                    Data = response,
                    Message = "Voucher was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Voucher"));
        }
        [HttpGet("GetVoucherByNo/{voucherNo}/{customerPhone}")]
        [ProducesResponseType(typeof(Response<VoucherDTO>), 200)]
        public async Task<IActionResult> GetVoucherByNo(string voucherNo, string customerPhone)
        {

            var response = await service.GetVoucherByVoucherNo(voucherNo, customerPhone);
            if (response != null)
                return Ok(new Response<VoucherDTO>
                {
                    Data = response,
                    Message = "Voucher was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Voucher"));
        }
    }
}

