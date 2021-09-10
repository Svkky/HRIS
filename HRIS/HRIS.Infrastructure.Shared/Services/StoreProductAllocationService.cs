using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.StoreProductAllocation;
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
    public class StoreProductAllocationService : IStoreProductAllocationRepository
    {
        private readonly ILogger<StoreProductAllocationService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        //  private readonly IOptions<ConnectionStrings> _connectionString;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;

        int IStoreProductAllocationRepository.CreateStoreAllocation(CreateStoreProductAllocationDTO model)
        {
            var param = new DynamicParameters();
            param.Add("Status", Status.INSERT);
            param.Add("BranchId", model.BranchId);
            param.Add("ProductId", model.ProductId);
            param.Add("AllocationQuantity", model.AllocationQuantity);
            param.Add("CreatedBy", _authenticatedUser.UserId);
            var response = _dapper.Execute(ApplicationConstant.Sp_AllocateProduct, param, CommandType.StoredProcedure);
            return response;
        }

        List<StoreProductAllocationDTO> IStoreProductAllocationRepository.GetAllStoreAllocation()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                var response = _dapper.GetAll<StoreProductAllocationDTO>(ApplicationConstant.Sp_AllocateProduct, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving Store Product Allocation list");
                return null;
            }
        }
    }
}
