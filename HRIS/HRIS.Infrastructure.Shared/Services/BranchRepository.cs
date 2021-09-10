using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Branch;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace HRIS.Infrastructure.Shared.Services
{
    public class BranchRepository : IBranchRepository
    {
        private readonly ILogger<BranchRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public BranchRepository(ILogger<BranchRepository> logger,
                                     IAuthenticatedUserService authenticatedUser,
                                     IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }

        public int CreateBranch(BranchDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("BranchName", model.branchName);
                param.Add("Location", model.location);
                param.Add("PhoneNumber", model.phoneNumber);
                param.Add("CreatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Branch, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving branch");
                return 0;
            }
        }

        public int DeleteBranch(int BranchID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("BranchID", BranchID);
                param.Add("DeletedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Branch, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting a branch");
                return 0;
            }
        }

        public int RemoveUserFromBranch(string UserID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", 7);
                param.Add("id", UserID);
                var response = _dapper.Execute(ApplicationConstant.Sp_Branch, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting a branch");
                return 0;
            }
        }

        public List<BranchDTO> GetAllBranch()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                var response = _dapper.GetAll<BranchDTO>(ApplicationConstant.Sp_Branch, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a list of Branch");
                return new List<BranchDTO>();
            }
        }

        public BranchDTO GetAllBranchByID(int BranchID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("BranchID", BranchID);
                var response = _dapper.Get<BranchDTO>(ApplicationConstant.Sp_Branch, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a Branch");
                return new BranchDTO();
            }
        }


        public List< BranchAdminDT> GetAllBranchAdmin(int BranchID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", 6);
                param.Add("BranchID", BranchID);
                var response = _dapper.GetAll<BranchAdminDT>(ApplicationConstant.Sp_Branch, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a Branch");
                return new List<BranchAdminDT>();
            }
        }

        public int UpdateBranch(UpdateBranchDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("PhoneNumber", model.PhoneNumber);
                param.Add("Location", model.Location);
                param.Add("BranchName", model.BranchName);
                param.Add("BranchID", model.BranchID);
                param.Add("UpdatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Branch, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Branch");
                return 0;
            }
        }

        public int AssignUserToBranch(AssignUserToBranchDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", 8);
                param.Add("BranchID", model.BranchID);
                param.Add("id", model.UserID);
                var response = _dapper.Execute(ApplicationConstant.Sp_Branch, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Branch");
                return 0;
            }
        }
    }
}
