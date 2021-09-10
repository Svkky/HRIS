using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.Category;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRIS.InventoryManager.WebAPI.Controllers.v1
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
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Post([FromBody] CreateCategoryDTO request)
        {
            var response = _categoryService.CreateCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating category"));
        }
        [HttpPut]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Put([FromBody] UpdateCategoryDTO request)
        {
            var response = _categoryService.UpdateCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating category"));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Delete([FromBody] DeleteCategoryDTO request)
        {
            var response = _categoryService.DeleteCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Category was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting category"));
        }
        [HttpGet]
        [ProducesResponseType(typeof(Response<List<CategoryDTO>>), 200)]
        public IActionResult Get()
        {
            var response = _categoryService.GetAllCategory();
            if (response != null)
                return Ok(new Response<List<CategoryDTO>>
                {
                    Data = response,
                    Message = "Category list was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving category"));
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
