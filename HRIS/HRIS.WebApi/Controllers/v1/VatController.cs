using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Vat;
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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VatController : ControllerBase
    {
        private readonly IVatService vatService;
        public VatController(IVatService vatService)
        {
            this.vatService = vatService;
        }

        //api/Vat/CreateVat
        [HttpPost("CreateVat")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateVat([FromBody] CreateVatDTO request)
        {
            var response = vatService.CreateVat(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Vat was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating Vat"));
        }

        //api/Vat/UpdateVat
        [HttpPost("UpdateVat")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateVat([FromBody] UpdateVatDTO request)
        {
            var response = vatService.UpdateVat(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Vat was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Vat"));
        }

        //api/Vat/RemoveVat
        [HttpPost("RemoveVat")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveVat([FromBody] DeleteVatDTO request)
        {
            var response = vatService.DeleteVat(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Vat was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Vat"));
        }

        //api/Vat/GetAllVat
        [HttpGet("GetAllVat")]
        [ProducesResponseType(typeof(Response<List<VatDTO>>), 200)]
        public IActionResult GetAllVat(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = vatService.GetAllVat(branchId);
            var responses = new Response<List<VatDTO>>();
            var vatList = new List<VatDTO>();

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Vat list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"vatId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Name), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failure to retrive vat";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Name), loadOptions));
        }

        //api/Vat/GetVat
        [HttpGet("GetVat")]
        [ProducesResponseType(typeof(Response<List<VatDTO>>), 200)]
        public IActionResult GetById([FromHeader] int VatId)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = vatService.GetVatById(VatId, branchId);
            if (response != null)
                return Ok(new Response<VatDTO>
                {
                    Data = response,
                    Message = "Vat was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Vat"));
        }

        //api/Vat/GetFirstVat
        [HttpGet("GetFirstVat")]
        public IActionResult GetFirstVat()
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var branches = vatService.GetAllVat(branchId);
            var response = branches.FirstOrDefault();
            return Ok(response != null ? response.percentage : 0.0);
        }
    }
}
