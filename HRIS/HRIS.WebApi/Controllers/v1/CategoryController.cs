using DevExtreme.AspNet.Data;
using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Category;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRIS.WebApi.Controllers.v1
{

    [ApiVersion("1.0")]
    [Authorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("CreateCategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult CreateCategory([FromBody] CreateCategoryDTO request)
        {
            var response = _categoryService.CreateCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was created successfully"));
            else if (response == 0)
                return Ok(ResponseHelper.FailureMessage("Failure creating category"));
            return Ok(ResponseHelper.AlreadyExistMessage("Category exists"));
        }

        [HttpPost]
        [Route("UpdateCategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult UpdateCategory([FromBody] UpdateCategoryDTO request)
        {
            var response = _categoryService.UpdateCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating category"));
        }

        [HttpPost("DeleteCategory")]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult DeleteCategory([FromBody] DeleteCategoryDTO request)
        {
            var response = _categoryService.DeleteCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting category"));
        }

        //v1.0/Category/GetAllCategory
        [HttpGet]
        [Route("GetAllCategory")]
        //[ProducesResponseType(typeof(Response<List<CategoryDTO>>), 200)]
        public IActionResult GetAllCategory(DataSourceLoadOptions loadOptions)
        {
            //var branchId = Request.Headers["branchId"].ToString();
            var response = _categoryService.GetAllCategory();
            var responses = new Response<List<CategoryDTO>>();
            var categoryList = new List<CategoryDTO>();
            if (response != null)
            {
                try
                {
                    responses.Data = response;
                    responses.Message = "Category list was retrieved successfully";
                    responses.ResponseCode = ApplicationConstant.SuccessResponseCode;
                    responses.StatusCode = ApplicationConstant.SuccessStatusCode;
                    responses.Succeeded = true;

                    loadOptions.PrimaryKey = new[] { $"CategoryId" };
                    return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Description), loadOptions));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            responses.Data = response;
            responses.Message = "Failed to retrive category";
            responses.ResponseCode = ApplicationConstant.FailureResponse;
            responses.StatusCode = ApplicationConstant.BadRequestStatusCode;
            responses.Succeeded = false;
            return Ok(DataSourceLoader.Load(responses.Data.OrderBy(x => x.Description), loadOptions));
        }

        [HttpGet]
        [Route("GetAllCategoryRecord")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAllCategoryRecord(DataSourceLoadOptions loadOptions)
        {
            var dtos = _categoryService.GetAllCategoryRecord();
            var result = new CategoryVm();

            try
            {
                loadOptions.PrimaryKey = new[] { "categoryId" };
                return Ok(DataSourceLoader.Load(dtos.recordResponseObject.OrderBy(x => x.Description), loadOptions));
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return Ok(DataSourceLoader.Load(result.recordResponseObject.OrderBy(x => x.Description), loadOptions));

        }

        [HttpGet("categoryId")]
        [ProducesResponseType(typeof(Response<List<CategoryDTO>>), 200)]
        public IActionResult GetById(int categoryId)
        {
            var response = _categoryService.GetCategoryById(categoryId);
            if (response != null)
                return Ok(new Response<CategoryDTO>
                {
                    Data = response,
                    Message = "Category was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving category"));
        }
    }
}
