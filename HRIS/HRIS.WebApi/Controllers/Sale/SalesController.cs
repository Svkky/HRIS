using DevExtreme.AspNet.Data;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Sales;
using HRIS.Application.DTOs.Warehouse;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.Sale
{
    [ApiVersion("1.0")]
    [Authorize]
    public class SalesController : BaseApiController
    {

        private readonly ISalesService salesService;
        private readonly IAuthenticatedUserService authenticatedUserService;

        public SalesController(ISalesService salesService, IAuthenticatedUserService authenticatedUserService)
        {
            this.salesService = salesService;
            this.authenticatedUserService = authenticatedUserService;
        }

        //[HttpPost]
        //[Route("CreateSales")]
        //[ProducesResponseType(typeof(Response<SalesVm>), 200)]
        //public IActionResult CreateSales([FromBody] ParentSales modelDTO)
        //{
        //    var response = salesService.CreateSales(modelDTO);
        //    var responses = new Response<SalesVm>();
        //    if (response != null)
        //    {
        //        try
        //        {
        //            responses.Data = response;
        //            responses.Message = $"Sale was completed successfully here is the bill number : {response.BillNumber}";
        //            responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
        //            responses.StatusCode = ApplicationConstant.SuccessStatusCode;
        //            responses.Succeeded = true;

        //            return Ok(responses);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.Write(ex.Message);
        //        }
        //    }
        //    responses.Data = response;
        //    responses.Message = "An error occured";
        //    responses.ResponseCode = ApplicationConstant.FailureResponse;
        //    responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
        //    responses.Succeeded = false;
        //    return Ok(responses);
        //}
        [HttpPost]
        [Route("CreateSales")]
        [ProducesResponseType(typeof(Response<SalesVm>), 200)]
        public async Task<IActionResult> CreateSales([FromBody] ParentSales modelDTO)
        {
            var response = await salesService.AddSaleAsync(modelDTO);
            var responses = new Response<SalesVm>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = $"Sale was completed successfully here is the bill number : {response.BillNumber}";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    return Ok(responses);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "An error occured";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(responses);
        }



        [HttpGet]
        [Route("GetSaleByBill/{bill}")]
        [ProducesResponseType(typeof(Response<SalesVm>), 200)]
        public async Task<IActionResult> GetSaleByBill(string bill)
        {
            var response = await salesService.GetAllSaleAsync(bill);
            var responses = new Response<SalesVm>();


            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Sales list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;


                    return Ok(responses);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }

            responses.Data = response;
            responses.Message = "Failure to retrive Sale List";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;

            return Ok(responses);

        }


        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetSales(DataSourceLoadOptions loadOptions)
        {
            var brnchId = Request.Headers["brnchId"].ToString();
            var from = Request.Headers["from"].ToString();
            var to = Request.Headers["to"].ToString();
            var ProductName = Request.Headers["ProductName"].ToString();
            var transactionType = Request.Headers["transactionType"].ToString();
            //var branchId = Request.Headers["branchId"].ToString();
            var response = new List<SalesVm>();
            if (ProductName != "" || from != "" || to != "" || transactionType != "")
            {
                var filter = new SearchFilter
                {
                    ProductName = ProductName == "" ? null : ProductName,
                    From = from != "" ? Convert.ToDateTime(from) : new DateTime(),
                    To = to != "" ? Convert.ToDateTime(to) : new DateTime(),
                    TransactionType = transactionType == "" ? null : transactionType
                };
                response = await salesService.FilterSales(filter, authenticatedUserService.UserId, brnchId);
            }
            else
            {
                response = await salesService.GetSales(authenticatedUserService.UserId, brnchId);
            }

            var responses = new Response<List<SalesVm>>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "sales list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"BillNumber" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.DatePaid), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = new List<SalesVm>();
            responses.Message = "Failed to retrive vendor payments";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.DatePaid), loadOptions));
        }
        [HttpGet]
        [Route("get-branch-sales")]
        public async Task<IActionResult> GetBranchSales(DataSourceLoadOptions loadOptions)
        {

            var response = await salesService.BranchSales();

            var responses = new Response<List<BranchSales>>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "sales list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"BranchId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.TotalAmount), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = new List<BranchSales>();
            responses.Message = "Failed to retrive vendor payments";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.TotalAmount), loadOptions));
        }

        [HttpGet]
        [Route("get-details")]
        public async Task<IActionResult> GetSalesByBill(DataSourceLoadOptions loadOptions)
        {
            var bill = Request.Headers["billNumber"].ToString();

            var response = await salesService.GetSalesDetails(bill);
            var responses = new Response<List<SalesDetailVM>>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "sales detail list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"SaleDetailId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.Name), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive sales details";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderByDescending(x => x.Name), loadOptions));
        }


    }
}
