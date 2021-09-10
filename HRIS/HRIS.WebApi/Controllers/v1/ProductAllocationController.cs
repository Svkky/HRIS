using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.ProductAllocation;
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
    public class ProductAllocationController : BaseApiController
    {
        private readonly IProductAllocationRepository _productAllocationRepository;

        public ProductAllocationController(IProductAllocationRepository productAllocationRepository)
        {
            _productAllocationRepository = productAllocationRepository;
        }

        [HttpPost]
        [Route("allocate")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult AllocatoProductToBranch([FromBody] CreateProductAllocationDTO request)
        {
            var response = _productAllocationRepository.CreateProductAllocation(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("product was allocated successfully "));
            return Ok(ResponseHelper.FailureMessage("Failure allocating product"));
        }

        [HttpGet]
        [Route("get-allocated-product")]
        [ProducesResponseType(typeof(Response<List<ProductAllocationDTO>>), 200)]
        public IActionResult GetAll(DataSourceLoadOptions loadOptions)
        {
            var response = _productAllocationRepository.GetProductAllocations();
            var responses = new Response<List<ProductAllocationDTO>>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "allocation list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"productAllocationId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive allocation list";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));
        }

    }
}
