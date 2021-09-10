using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.SubCategory;
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
    public class SubCategoryController : BaseApiController
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        //SubCategory
        [HttpPost]
        [Route("CreateSubCategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateSubCategory([FromBody] CreateSubCategoryDTO request)
        {
            var response = _subCategoryService.CreateSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subcategory was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating subcategory"));
            return Ok(ResponseHelper.AlreadyExistMessage("Subcategory exists"));
        }


        [HttpPost]
        [Route("UpdateSubCategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateSubCategory([FromBody] UpdateSubCategoryDTO request)
        {
            var response = _subCategoryService.UpdateSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subategory was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating subcategory"));
        }

        [HttpPost]
        [Route("DeleteSubcategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteSubcategory([FromBody] DeleteSubCategoryDTO request)
        {
            var response = _subCategoryService.DeleteSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subcategory was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting subcategory"));
        }


        [HttpGet("GetAllSubCategory")]
        [ProducesResponseType(typeof(Response<List<SubCategoryDTO>>), 200)]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            //var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _subCategoryService.GetAllSubCategory();
            var responses = new Response<List<SubCategoryDTO>>();
            var categoryList = new List<SubCategoryDTO>();
            if (response != null)
            {

                try
                {
                    responses.Data = response;
                    responses.Message = "Subcategory list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"CategoryId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.SubCategoryName), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive Subcategory";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.SubCategoryName), loadOptions));
        }

        //api/SubCategory/GetSubCategoryById
        [HttpGet]
        [Route("GetSubCategoryById/{subCategoryId}")]
        [ProducesResponseType(typeof(Response<List<SubCategoryVm>>), 200)]
        public IActionResult GetSubCategoryById(int subCategoryId)
        {
            //var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _subCategoryService.GetSubCategoryById(subCategoryId);
            if (response != null)
                return Ok(new Response<List<SubCategoryVm>>
                {
                    Data = response,
                    Message = "Subcategory was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving subcategory"));
        }

        [HttpGet]
        [Route("GetSubCategory/{Id}")]
        [ProducesResponseType(typeof(Response<List<SubCategoryVm>>), 200)]
        public IActionResult GetSubCategory(int Id)
        {
            //var branchId = int.Parse(Request.Headers["branchId"].ToString());
            var response = _subCategoryService.GetSubCategory(Id);
            if (response != null)
                return Ok(new Response<List<SubCategoryVm>>
                {
                    Data = response,
                    Message = "Subcategory was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving subcategory"));
        }
    }
}
