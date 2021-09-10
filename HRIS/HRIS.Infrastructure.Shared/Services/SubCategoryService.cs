using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.SubCategory;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class SubSubCategoryService : ISubCategoryService
    {
        private readonly ILogger<SubSubCategoryService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public SubSubCategoryService(ILogger<SubSubCategoryService> logger,
                                     IAuthenticatedUserService authenticatedUser,
                                     IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }
        public int CreateSubCategory(CreateSubCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("CategoryId", model.CategoryId);
                param.Add("Description", model.Description);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                //param.Add("BranchId", model.BranchId);

                var response = _dapper.Execute(ApplicationConstant.Sp_SubCategory, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving SubCategory");
                return 0;
            }
        }

        public int DeleteSubCategory(DeleteSubCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("SubCategoryId", model.SubCategoryId);
                param.Add("DeletedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_SubCategory, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting a category");
                return 0;
            }
        }

        public List<SubCategoryDTO> GetAllSubCategory()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                //param.Add("BranchId", branchId);
                var response = _dapper.GetAll<SubCategoryDTO>(ApplicationConstant.Sp_SubCategory, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a list of SubCategory");
                return new List<SubCategoryDTO>();
            }
        }

        public List<SubCategoryVm> GetSubCategoryById(int SubCategoryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("CategoryID", SubCategoryId);
                //param.Add("BranchId", branchId);
                var response = _dapper.GetAll<SubCategoryVm>(ApplicationConstant.Sp_SubCategory, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a SubCategory");
                return new List<SubCategoryVm>();
            }
        }

        public List<SubCategoryVm> GetSubCategory(int SubCategoryId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETSUBID);
                param.Add("SubCategoryID", SubCategoryId);
                //param.Add("BranchId", branchId);
                var response = _dapper.GetAll<SubCategoryVm>(ApplicationConstant.Sp_SubCategory, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a SubCategory");
                return new List<SubCategoryVm>();
            }
        }

        public int UpdateSubCategory(UpdateSubCategoryDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("CategoryId", model.CategoryId);
                param.Add("SubCategoryId", model.SubCategoryId);
                param.Add("Description", model.Description);
                param.Add("UpdatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_SubCategory, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a SubCategory");
                return 0;
            }
        }
    }
}
