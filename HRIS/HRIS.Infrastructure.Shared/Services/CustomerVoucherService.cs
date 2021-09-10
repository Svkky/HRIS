using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.CustomerVoucher;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class CustomerVoucherService : ICustomerVoucherService
    {
        private readonly ILogger<CustomerVoucherService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public CustomerVoucherService(ILogger<CustomerVoucherService> logger,
                                     IAuthenticatedUserService authenticatedUser,
                                     IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }
        public int CreateCustomerVoucher(CreateCustomerVoucherDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("CustomerId", model.CustomerId);
                param.Add("VoucherId", model.VoucherId);
                param.Add("BranchId", model.BranchId);
                param.Add("CreatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_CustomerVoucher, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving CustomerVoucher");
                return 0;
            }
        }

        public int DeleteCustomerVoucher(DeleteCustomerVoucherDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("CustomerVoucherId", model.customerVoucherId);
                param.Add("DeletedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_CustomerVoucher, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting a category");
                return 0;
            }
        }

        public List<CustomerVoucherDTO> GetAllCustomerVoucher(int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", branchId);
                var response = _dapper.GetAll<CustomerVoucherDTO>(ApplicationConstant.Sp_CustomerVoucher, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a list of CustomerVoucher");
                return new List<CustomerVoucherDTO>();
            }
        }

        public CustomerVoucherDTO GetCustomerVoucherById(int CustomerVoucherId,int branchId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("BranchId", branchId);
                param.Add("CustomerVoucherId", CustomerVoucherId);
                var response = _dapper.Get<CustomerVoucherDTO>(ApplicationConstant.Sp_CustomerVoucher, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a CustomerVoucher");
                return new CustomerVoucherDTO();
            }
        }



    }
}
