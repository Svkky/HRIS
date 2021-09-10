using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Warehouse;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers
{

    [ApiVersion("1.0")]
    [Authorize(Roles = "StoreAdmin")]
    public class ProductWarehouseController : BaseApiController
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public ProductWarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }
        [HttpPost]
        [Route("add-product")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> AddProduct([FromBody] AddProductToWareHouseDTO request)
        {
            var response = await _warehouseRepository.AddProductToWarehouse(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("product was added to the warehouse successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure adding product to warehouse"));
            return Ok(ResponseHelper.AlreadyExistMessage("Failure adding product to warehouse"));
        }

        [HttpGet]
        [Route("get-all-vendor-payment")]
        public async Task<IActionResult> GetAllVendorPayment(DataSourceLoadOptions loadOptions)
        {
            var from = Request.Headers["from"].ToString();
            var to = Request.Headers["to"].ToString();
            var SupplierId = Request.Headers["SupplierId"].ToString();
            //var branchId = Request.Headers["branchId"].ToString();
            var response = new List<VendorPaymentDTO>();
            if (SupplierId != "" || from != "" || to != "")
            {
                var filter = new SearchFilter
                {
                    SupplierId = SupplierId,
                    From = from != "" ? Convert.ToDateTime(from) : new DateTime(),
                    To = to != "" ? Convert.ToDateTime(to) : new DateTime()
                };
                response = await _warehouseRepository.FilterPayments(filter);
            }
            else
            {
                response = await _warehouseRepository.GetVendorPayments();
            }

            var responses = new Response<List<VendorPaymentDTO>>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Vendor Payment list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"VendorPaymentMasterId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.BillNo), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive vendor payments";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.BillNo), loadOptions));
        }
        [HttpGet]
        [Route("get-products")]
        public async Task<IActionResult> GetProductsInWarehouse(DataSourceLoadOptions loadOptions)
        {
            var from = Request.Headers["from"].ToString();
            var to = Request.Headers["to"].ToString();
            var ProductName = Request.Headers["ProductName"].ToString();
            var transactionType = Request.Headers["transactionType"].ToString();
            //var branchId = Request.Headers["branchId"].ToString();
            var response = new List<ProductWareHouseDTO>();
            if (ProductName != "" || from != "" || to != "" || transactionType != "")
            {
                var filter = new SearchFilter
                {
                    ProductName = ProductName == "" ? null : ProductName,
                    From = from != "" ? Convert.ToDateTime(from) : new DateTime(),
                    To = to != "" ? Convert.ToDateTime(to) : new DateTime(),
                    TransactionType = transactionType == "" ? null : transactionType
                };
                response = await _warehouseRepository.FilterProductsInWarehouse(filter);
            }
            else
            {
                response = await _warehouseRepository.GetWareHouseProduct();
            }

            var responses = new Response<List<ProductWareHouseDTO>>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "product list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"ProductWareHouseId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.DateCreated), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive vendor payments";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.DateCreated), loadOptions));
        }

        [HttpGet("get-product-count/{productId}")]
        [ProducesResponseType(typeof(Response<List<int>>), 200)]
        public async Task<IActionResult> GetById(int productId)
        {
            var response = await _warehouseRepository.GetTotalRemainingProduct(productId);
            if (response != -1)
                return Ok(new Response<int>
                {
                    Data = response,
                    Message = "Product count retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Product count"));
        }
    }
}
