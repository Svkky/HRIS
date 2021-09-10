using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Base;
using HRIS.Application.DTOs.Customer;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;


namespace HRIS.Infrastructure.Shared.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ILogger<CustomerService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public CustomerService(ILogger<CustomerService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }


        public int CreateCustomer(CreateCustomerDTO model)
        {
            BaseResponse Resp = new BaseResponse();

            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("FullName", model.FullName);
                param.Add("Email", model.Email);
                param.Add("Phone", model.Phone);
                param.Add("Address", model.Address);
                param.Add("BranchId", model.BranchId);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_Customer, param, CommandType.StoredProcedure);

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving Customer");
                return 0;
            }
        }

        public int DeleteCustomer(DeleteCustomerDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("CustomerId", model.CustomerId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Customer, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting Customer");
                return 0;
            }
        }

        public List<CustomerDTO> GetAllCustomer(int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", branchId);
                var response = _dapper.GetAll<CustomerDTO>(ApplicationConstant.Sp_Customer, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Customer");
                return null;
            }
        }

        public CustomerDTO GetCustomerById(int CustomerId,int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("CustomerId", CustomerId);
                param.Add("BranchId", branchId);
                var response = _dapper.Get<CustomerDTO>(ApplicationConstant.Sp_Customer, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Customer");
                return new CustomerDTO();
            }
        }

        public int UpdateCustomer(UpdateCustomerDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("CustomerId", model.CustomerId);
                param.Add("FullName", model.FullName);
                param.Add("Email", model.Email);
                param.Add("Phone", model.Phone);
                param.Add("Address", model.Address);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_Customer, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Customer");
                return 0;
            }
        }

        public CustomerDTO getCustomerByPhone(string Phone,int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYPHONE);
                param.Add("Phone", Phone);
                param.Add("branchId", branchId);
                var response = _dapper.Get<CustomerDTO>(ApplicationConstant.Sp_Customer, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a Customer");
                return new CustomerDTO();
            }
        }
    }
}
