using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.ProductAllocation;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class ProductAllocationRepository : IProductAllocationRepository
    {
        private readonly ILogger<ProductAllocationRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public ProductAllocationRepository(ILogger<ProductAllocationRepository> logger,
                                     IAuthenticatedUserService authenticatedUser,
                                     IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }
        public int CreateProductAllocation(CreateProductAllocationDTO request)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("BranchId", request.BranchId);
                param.Add("ProductId", request.ProductId);
                param.Add("AllocationQuantity", request.AllocationQuantity);
                param.Add("CreatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_AllocateProduct, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving branch");
                return 0;
            }
        }

        public List<ProductAllocationDTO> GetProductAllocations()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                var response = _dapper.GetAll<ProductAllocationDTO>(ApplicationConstant.Sp_AllocateProduct, param, commandType: CommandType.StoredProcedure);
                response.ForEach(c => c.DateCreated = c.CreatedOn.ToShortDateString());
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a list of Branch");
                return new List<ProductAllocationDTO>();
            }
        }
    }
}
