using HRIS.Application.ApiResponseHelpers;
using HRIS.Application.AppContants;
using HRIS.Application.DTOs.SubCategory;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HRIS.InventoryManager.WebAPI.Controllers.v1
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
        [HttpPost]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Post([FromBody] CreateSubCategoryDTO request)
        {
            var response = _subCategoryService.CreateSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subcategory was created successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure creating subcategory"));
        }
        [HttpPut]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Put([FromBody] UpdateSubCategoryDTO request)
        {
            var response = _subCategoryService.UpdateSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subategory was updated successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure updating subcategory"));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(Response<string>), 200)]
        public IActionResult Delete([FromBody] DeleteSubCategoryDTO request)
        {
            var response = _subCategoryService.DeleteSubCategory(request);
            if (response > 0)
                return Ok(ResponseHelper.SuccessMessage("Subcategory was deleted successfully"));
            return Ok(ResponseHelper.FailureMessage("Failure deleting subcategory"));
        }
        [HttpGet]
        [ProducesResponseType(typeof(Response<List<SubCategoryDTO>>), 200)]
        public IActionResult Get()
        {
            var response = _subCategoryService.GetAllSubCategory();
            if (response != null)
                return Ok(new Response<List<SubCategoryDTO>>
                {
                    Data = response,
                    Message = "Subcategory list was retrieved successfully",
                    ResponseCode = ApplicationConstant.SuccessResponseCode,
                    StatusCode = ApplicationConstant.SuccessStatusCode,
                    Succeeded = true
                });
            return Ok(ResponseHelper.FailureMessage("Failure retrieving subcategory"));
        }
        [HttpGet("subCategoryId")]
        [ProducesResponseType(typeof(Response<SubCategoryDTO>), 200)]
        public IActionResult GetById(int subCategoryId)
        {
            var response = _subCategoryService.GetSubCategoryById(subCategoryId);
            if (response != null)
                return Ok(new Response<SubCategoryDTO>
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
