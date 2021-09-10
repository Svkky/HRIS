using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Products;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Wrappers;
using HRIS.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.Product
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ProductsController : BaseApiController
    {
        private readonly IProductsRepository _productsRepository;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public ProductsController(IProductsRepository productsRepository,
                                  IAuthenticatedUserService authenticatedUserService)
        {
            _productsRepository = productsRepository;
            _authenticatedUserService = authenticatedUserService;
        }

        //[HttpPost]
        //[Route("CreateProduct")]
        //[ProducesResponseType(typeof(Response<string>), 200)]
        //public IActionResult CreateProduct([FromBody] BranchProductDTO request)
        //{
        //    var response = _productsRepository.CreateProduct(request);
        //    if (response > 0)
        //        return Ok(ResponseHelper.SuccessMessage("Product was created successfully"));
        //    return Ok(ResponseHelper.FailureMessage("Failure creating Product"));
        //}

        [HttpPost]
        [Route("UpdateProduct")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> UpdateProduct([FromBody] EditBranchProductDTO request)
        {
            var response = await _productsRepository.UpdateProductAsync(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Product was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Product"));
        }

        //[HttpPost]
        //[Route("RemoveProduct")]
        //[ProducesResponseType(typeof(Response<string>), 200)]
        //public IActionResult RemoveProduct([FromBody] DeleteProductDto model)
        //{
        //    var response = _productsRepository.DeleteProduct(model.productID);
        //    if (response > 0)
        //        return Ok(ResponseHelper.SuccessMessage("Product was deleted successfully"));
        //    return Ok(ResponseHelper.FailureMessage("Failure deleting Product"));
        //}


        [HttpGet]
        [Route("get-allocated-branch-products")]
        [ProducesResponseType(typeof(Response<List<BranchProductDTO>>), 200)]
        public async Task<IActionResult> GetAllProduct(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = await _productsRepository.GetAllProducts(branchId);
            var responses = new Response<List<BranchProductDTO>>();

            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Product list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"BranchProductId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive Product";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));


        }

        //Used by dev express dropdown
        [AllowAnonymous]
        [HttpGet]
        [Route("get-allocated-branchproducts")]
        [ProducesResponseType(typeof(Response<List<BranchProductDTO>>), 200)]
        public async Task<IActionResult> GetProducts(DataSourceLoadOptions loadOptions)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = await _productsRepository.GetAllProducts(branchId);

            loadOptions.PrimaryKey = new[] { $"BranchProductId" };
            return Ok(DataSourceLoader.Load(response.OrderBy(x => x.ProductName), loadOptions));
        }
        //[HttpGet]
        //[Route("get-allocated-branch-products")]
        //[ProducesResponseType(typeof(Response<List<ProductAllocationDTO>>), 200)]
        //public async Task<IActionResult> GetProductsAllocatedToBranch(DataSourceLoadOptions loadOptions)
        //{
        //    var branchId = int.Parse(Request.Headers["branchId"].ToString());
        //    var response = await _productsRepository.GetBranchProducts(branchId);
        //    var responses = new Response<List<ProductAllocationDTO>>();

        //    if (response != null)
        //    {
        //        try
        //        {
        //            responses.Data = response;
        //            responses.Message = "Product list was retrieved successfully";
        //            responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
        //            responses.StatusCode = ApplicationConstant.SuccessStatusCode;
        //            responses.Succeeded = true;

        //            loadOptions.PrimaryKey = new[] { $"productId" };
        //            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }
        //    }
        //    responses.Data = response;
        //    responses.Message = "Failed to retrive Product";
        //    responses.ResponseCode = ApplicationConstant.FailureResponse;
        //    responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
        //    responses.Succeeded = false;
        //    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.ProductName), loadOptions));


        //}

        [HttpGet]
        [Route("GetById/{productId}")]
        [ProducesResponseType(typeof(Response<BranchProductDTO>), 200)]
        public async Task<IActionResult> GetById(int productId)
        {
            var branchId = int.Parse(_authenticatedUserService.BranchId);
            var response = await _productsRepository.GetProductByID(productId, branchId);
            if (response != null)
                return Ok(new Response<BranchProductDTO>
                {
                    Data = response,
                    Message = "Product was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Products"));
        }

    }
}
