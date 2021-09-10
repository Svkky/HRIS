using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs;
using HRIS.Application.DTOs.Store;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebApi.Controllers.StoreSetup
{
    [ApiVersion("1.0")]
    [Authorize]
    public class StoreSetupController : BaseApiController
    {
        private readonly IStoreSetupRepository _StoreRepository;

        public StoreSetupController(IStoreSetupRepository StoreRepository)
        {
            _StoreRepository = StoreRepository;
        }


        [HttpPost]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public async Task<IActionResult> Post([FromBody] StoreSetUpDTO request)
        {
            var response = await _StoreRepository.CreateStore(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Store was created successfully"));
            else if (response == -12)
                return Ok(ResponseHelper.FailureMessage("Account created but system could not send confirmation email to the user. Please click on the resend confirmation email button"));
            return Ok(ResponseHelper.FailureMessage("User Already has a Store"));


        }

        [HttpPost]
        [Route("UpdateStore")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateStore([FromBody] StoreUpdateDTO request)
        {
            var response = _StoreRepository.UpdateStore(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Store was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Store"));
        }


        //[HttpGet]
        //[ProducesResponseType(typeof(Response<List<StoreSetUpDTO>>), 200)]
        [HttpGet]
        [Route("GetAllStores")]
        public IActionResult GetAllStores(DataSourceLoadOptions loadOptions)
        {
            var response = _StoreRepository.GetAllStores();
            var responses = new Response<List<StoreSetUpDTO>>();
            var categoryList = new List<StoreSetUpDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Store list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;
                    loadOptions.PrimaryKey = new[] { $"storeSetupId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.CreatedOn), loadOptions));
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive Store";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.CreatedOn), loadOptions));
        }

        [HttpGet("StoreID")]
        [ProducesResponseType(typeof(Response<List<StoreSetUpDTO>>), 200)]
        public IActionResult GetById(int StoreID)
        {
            var response = _StoreRepository.GetStoreByID(StoreID);
            if (response != null)
                return Ok(new Response<StoreSetUpDTO>
                {
                    Data = response,
                    Message = "Store was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Store"));
        }
    }
}
