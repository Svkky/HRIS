using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Expenditure;
using HRIS.Application.Helpers;
using HRIS.Application.Wrappers;
using HRIS.Infrastructure.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS.WebApi.Controllers.Expenditure
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ExpenditureController : BaseApiController
    {
        private readonly IExpenditureRepository _ExpenditureRepository;

        public ExpenditureController(IExpenditureRepository ExpenditureRepository)
        {
            _ExpenditureRepository = ExpenditureRepository;

        }
        [HttpPost]
        [Route("CreateExpenditure")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateExpenditure([FromBody] CreateExpenditureDTO request)
        {
            var response = _ExpenditureRepository.CreateExpenditure(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Expenditure added successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure adding expenditure"));
            return Ok(ResponseHelper.AlreadyExistMessage("expenditur already exists"));
        }

        [HttpPost]
        [Route("UpdateExpenditure")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateExpenditure([FromBody] UpdateExpenditureDTO request)
        {
            var response = _ExpenditureRepository.UpdateExpenditure(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Expenditure was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Expenditure"));
        }

        [HttpPost]
        [Route("DeleteExpenditure")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteExpenditure([FromBody] DeleteExpenditureDTO request)
        {
            var response = _ExpenditureRepository.DeleteExpenditure(request.ExpenditureID);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Expenditure was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Expenditure"));
        }
        [HttpGet]
        [Route("GetAllExpendture")]
        [ProducesResponseType(typeof(Response<List<ExpenditureDTO>>), 200)]
        public IActionResult GetAllExpendture(DataSourceLoadOptions loadOptions)
        {
            var response = _ExpenditureRepository.GetAllExpenditure();
            var responses = new Response<List<ExpenditureDTO>>();
            if (response != null)
            {
                try
                {
                    response.ForEach(c => c.ExpenditureDatee = c.ExpenditureDate.ToString("dd/MM/yyyy"));
                    responses.Data = response;
                    responses.Message = "Expenditure list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"ExpenditureID" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Description), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive Expenditure";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Description), loadOptions));
        }



        [HttpGet]
        [Route("GetById/{ExpenditureID}")]
        [ProducesResponseType(typeof(Response<List<ExpenditureDTO>>), 200)]
        public IActionResult GetById(int ExpenditureID)
        {
            var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _ExpenditureRepository.GetExpenditureByID(ExpenditureID, branchId);
            if (response != null)
                return Ok(new Response<ExpenditureDTO>
                {
                    Data = response,
                    Message = "Expenditure was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Expenditure"));
        }
    }
}
