using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.StoreProductAllocation;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HRIS.WebApi.Controllers.StoreProductAllocation
{
    [ApiVersion("1.0")]
    [Authorize]
    public class StoreProductAllocationController : BaseApiController
    {
        private readonly IStoreProductAllocationRepository _StoreProductAllocationRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public StoreProductAllocationController(IStoreProductAllocationRepository StoreProductAllocationRepository,
                                  IAuthenticatedUserService authenticatedUserService)
        {
            _StoreProductAllocationRepository = StoreProductAllocationRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpPost]
        [Route("CreateStoreProductAllocation")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateStoreProductAllocation([FromBody] CreateStoreProductAllocationDTO request)
        {
            var response = _StoreProductAllocationRepository.CreateStoreAllocation(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("StoreProduct Allocation was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating StoreProduct Allocation"));
        }

        [HttpGet]
        [Route("GetAllStoreProductAllocation")]
        [ProducesResponseType(typeof(Response<List<StoreProductAllocationDTO>>), 200)]
        public IActionResult GetAllStoreProductAllocation(DataSourceLoadOptions loadOptions)
        {
            var response = _StoreProductAllocationRepository.GetAllStoreAllocation();
            var responses = new Response<List<StoreProductAllocationDTO>>();

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "StoreProduct Allocation list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"ProductAllocationId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductAllocationId), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive StoreProduct Allocation list";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));


        }
    }
}

