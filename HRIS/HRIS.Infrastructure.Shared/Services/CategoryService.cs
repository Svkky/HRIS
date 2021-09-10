using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Category;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        //  private readonly IOptions<ConnectionStrings> _connectionString;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;

        public CategoryService(ILogger<CategoryService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IOptions<ConnectionStrings> connectionString,
                               IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            constring = myconnectionString.Value.DefaultConnection;
        }


        public int CreateCategory(CreateCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Description", model.Description);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Category, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving category");
                return 0;
            }
        }

        public int DeleteCategory(DeleteCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("CategoryId", model.CategoryId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Category, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting category");
                return 0;
            }
        }

        public List<CategoryDTO> GetAllCategory()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                var response = _dapper.GetAll<CategoryDTO>(ApplicationConstant.Sp_Category, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  category");
                return null;
            }
        }
        public CategoryVm GetAllCategoryRecord()
        {
            var result = new CategoryVm();
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                //param.Add("BranchId", branchId);
                var response = _dapper.GetAll<CategoryDTO>(ApplicationConstant.Sp_Category, param, commandType: CommandType.StoredProcedure);
                result.isSuccessful = true;
                result.statusId = 200;
                result.recordResponseObject = response;
                result.statusMessage = "Successfully retrieved category list";

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  category");
                return result;
            }

        }

        public CategoryDTO GetCategoryById(int categoryId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("CategoryId", categoryId);
                //param.Add("BranchId", branchId);

                var response = _dapper.Get<CategoryDTO>(ApplicationConstant.Sp_Category, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return new CategoryDTO();
            }
        }

        public int UpdateCategory(UpdateCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("CategoryId", model.CategoryId);
                param.Add("Description", model.Description);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Category, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return 0;
            }
        }
    }
}
