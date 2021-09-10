using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Branch;
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
using System.Xml.Linq;

namespace HRIS.WebApi.Controllers.Branch
{
    [ApiVersion("1.0")]
    [Authorize]
    public class BranchController : BaseApiController
    {

        private readonly IBranchRepository _BranchRepository;
        private readonly IAccountService _accountService;
        private readonly IAuthenticatedUserService _authenticatedUserService;

        public BranchController(IBranchRepository BranchRepository, IAccountService accountService, IAuthenticatedUserService authenticatedUserService)
        {
            _BranchRepository = BranchRepository;
            _accountService = accountService;
            _authenticatedUserService = authenticatedUserService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Post([FromBody] BranchDTO request)
        {
            var response = _BranchRepository.CreateBranch(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Branch was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating Branch"));
        }

        [HttpPost]
        [Route("UpdateBranch")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateBranch([FromBody] UpdateBranchDTO request)
        {
            var response = _BranchRepository.UpdateBranch(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Branch was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating Branch"));
        }

        [HttpPost]
        [Route("AssignUserToBranch")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult AssignUserToBranch([FromBody] AssignUserToBranchDTO request)
        {
            var response = _BranchRepository.AssignUserToBranch(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("User Assign to Branch  successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure Assigning Branch to User"));
        }

        [HttpPost("DeleteBranch")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Delete(DeleteBranchDTO branch)
        {
            var response = _BranchRepository.DeleteBranch(branch.branchID);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Branch was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Branch"));
        }
        [HttpPost("RemoveUserFromBranch/{userId}")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult RemoveUserFromBranch(string userId)
        {
            var response = _BranchRepository.RemoveUserFromBranch(userId);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Branch was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting Branch"));
        }



        [HttpGet]
        [Route("GetAllBranch")]
        public IActionResult GetAllBranch(DataSourceLoadOptions loadOptions)
        {
            var response = _BranchRepository.GetAllBranch();
            var responses = new Response<List<BranchDTO>>();
            var categoryList = new List<BranchDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Branch list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;
                    loadOptions.PrimaryKey = new[] { $"branchID" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.createdOn), loadOptions));
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
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.createdOn), loadOptions));
        }


        [HttpGet("GetAllBranchByID/{BranchID}")]
        [ProducesResponseType(typeof(Response<List<BranchDTO>>), 200)]
        public IActionResult GetById(int BranchID)
        {
            var response = _BranchRepository.GetAllBranchByID(BranchID);
            if (response != null)
                return Ok(new Response<BranchDTO>
                {
                    Data = response,
                    Message = "Branch was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving BranchID"));
        }


        [HttpGet]
        [Route("GetAllBranchAdmin")]
        public IActionResult GetAllBranchAdmin(DataSourceLoadOptions loadOptions,[FromHeader]int id)
        {
           

            var response = _BranchRepository.GetAllBranchAdmin(id);
            var responses = new Response<List<BranchAdminDT>>();
            var categoryList = new List<BranchAdminDT>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Branch Admin was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;
                    loadOptions.PrimaryKey = new[] { $"branchID" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
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
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.FirstName), loadOptions));
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(Response<List<UserDTO>>), 200)]
        public async Task <IActionResult> GetUsers()
        {
            var response =  await _accountService.GetUsers(_authenticatedUserService.UserId);
            //var responses = new Response<List<UserDTO>>();
            if (response != null)
            {


                try
                {
                    return Ok(new Response<List<UserDTO>>
                    {
                        Data = response,
                        Message = "User  was retrieved successfully ",
                        ResponseCode = ApplicationConstant.SuccessResponseCode,
                        StatusCode = ApplicationConstant.SuccessStatusCode,
                        Succeeded = true
                    });
                }
                catch (Exception)
                {

                    throw;
                }
           
            }
            return Ok(ResponseHelper.FailureMessage("Failure retrieving Menulist"));
        }
    }
}
