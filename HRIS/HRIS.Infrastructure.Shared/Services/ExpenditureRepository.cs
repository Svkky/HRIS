using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Expenditure;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Domain.Common;
using HRIS.Infrastructure.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class ExpenditureRepository : IExpenditureRepository
    {

        private readonly ILogger<ExpenditureRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        public ExpenditureRepository(ILogger<ExpenditureRepository> logger,
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

        public int CreateExpenditure(CreateExpenditureDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Description", model.Description);
                param.Add("Amount", model.Amount);
                param.Add("BranchId", _authenticatedUser.BranchId);
                param.Add("Comment", model.Comment);

                param.Add("ExpenditureDate", DateTime.Parse(model.ExpenditureDate));
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Expenditure, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving Expenditure");
                return 0;
            }
        }

        public int DeleteExpenditure(int ExpenditureID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("ExpenditureID", ExpenditureID);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Expenditure, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting Expenditure");
                return 0;
            }
        }

        public List<ExpenditureDTO> GetAllExpenditure()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", _authenticatedUser.BranchId);
                var response = _dapper.GetAll<ExpenditureDTO>(ApplicationConstant.Sp_Expenditure, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Expenditure");
                return null;
            }
        }

        public ExpenditureDTO GetExpenditureByID(int ExpenditureID, int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("ExpenditureID", ExpenditureID);
                param.Add("BranchId", branchId);
                var response = _dapper.Get<ExpenditureDTO>(ApplicationConstant.Sp_Expenditure, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Expenditure");
                return new ExpenditureDTO();
            }
        }

        public int UpdateExpenditure(UpdateExpenditureDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("Amount", model.Amount);
                param.Add("Comment", model.Comment);
                param.Add("ExpenditureID", model.ExpenditureID);
                param.Add("Description", model.Description);
                param.Add("UpdatedOn", model.UpdatedOn);
                param.Add("UpdatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Expenditure, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Expenditure");
                return 0;
            }
        }

    }
}
