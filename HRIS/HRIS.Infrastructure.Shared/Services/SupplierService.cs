using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Supplier;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ILogger<SupplierService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public SupplierService(ILogger<SupplierService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }


        public int CreateSupplier(CreateSupplierDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Name", model.Name);
                param.Add("Email", model.Email);
                param.Add("Phone", model.Phone);
                param.Add("Address", model.Address);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Supplier, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving Supplier");
                return 0;
            }
        }

        public int DeleteSupplier(DeleteSupplierDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("SupplierId", model.SupplierId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Supplier, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting Supplier");
                return 0;
            }
        }

        public List<SupplierDTO> GetAllSupplier()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                //param.Add("BranchId", branchId);
                var response = _dapper.GetAll<SupplierDTO>(ApplicationConstant.Sp_Supplier, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Supplier");
                return null;
            }
        }

        public SupplierDTO GetSupplierById(int supplierId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("SupplierId", supplierId);
                //param.Add("BranchId", branchId);
                var response = _dapper.Get<SupplierDTO>(ApplicationConstant.Sp_Supplier, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Supplier");
                return new SupplierDTO();
            }
        }

        public int UpdateSupplier(UpdateSupplierDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("SupplierId", model.SupplierId);
                param.Add("Name", model.Name);
                param.Add("Email", model.Email);
                param.Add("Phone", model.Phone);
                param.Add("Address", model.Address);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Supplier, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Supplier");
                return 0;
            }
        }
    }
}
