using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.StoreProduct;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS.WebApi.Controllers.StoreProduct
{
    [ApiVersion("1.0")]
    [Authorize]
    public class StoreProductController : BaseApiController
    {
        private readonly IStoreProductService _StoreProductRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public StoreProductController(IStoreProductService StoreProductRepository,
                                  IAuthenticatedUserService authenticatedUserService)
        {
            _StoreProductRepository = StoreProductRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpPost]
        [Route("CreateStoreProduct")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateStoreProduct([FromBody] CreateStoreProductDTO request)
        {
            var response = _StoreProductRepository.CreateStoreProduct(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Product was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating product"));
            return Ok(ResponseHelper.AlreadyExistMessage("Product already exists"));
        }

        [HttpPost]
        [Route("UpdateStoreProduct")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateStoreProduct([FromBody] UpdateStoreProductDTO request)
        {
            var response = _StoreProductRepository.UpdateStoreProduct(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("StoreProduct was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating StoreProduct"));
        }

        [HttpPost]
        [Route("RemoveStoreProduct")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveStoreProduct([FromBody] DeleteStoreProductDTO model)
        {
            var response = _StoreProductRepository.DeleteStoreProduct(model.StoreProductID);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("StoreProduct was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting StoreProduct"));
        }


        [HttpGet]
        [Route("GetAllStoreProduct")]
        [ProducesResponseType(typeof(Response<List<StoreProductDTO>>), 200)]
        public IActionResult GetAllStoreProduct(DataSourceLoadOptions loadOptions)
        {
            var response = _StoreProductRepository.GetAllStoreProducts();
            var responses = new Response<List<StoreProductDTO>>();

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "StoreProduct list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"StoreProductId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive StoreProduct";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));


        }

        //Vendor DevExpress
        [HttpGet]
        [Route("get-all-store-products")]
        [ProducesResponseType(typeof(Response<List<StoreProductDTO>>), 200)]
        public IActionResult GetStoreProducts(DataSourceLoadOptions loadOptions)
        {
            var response = _StoreProductRepository.GetAllStoreProducts();
            loadOptions.PrimaryKey = new[] { $"StoreProductId" };
            return Ok(DataSourceLoader.Load(response.OrderBy(x => x.ProductName), loadOptions));
        }

        [HttpGet]
        [Route("GetById/{StoreProductId}")]
        [ProducesResponseType(typeof(Response<List<StoreProductDTO>>), 200)]
        public IActionResult GetById(int StoreProductId)
        {
            var branchId = int.Parse(_authenticatedUserService.BranchId);
            var response = _StoreProductRepository.GetStoreProductByID(StoreProductId);
            if (response != null)
                return Ok(new Response<StoreProductDTO>
                {
                    Data = response,
                    Message = "StoreProduct was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving StoreProduct"));
        }

    }
}
