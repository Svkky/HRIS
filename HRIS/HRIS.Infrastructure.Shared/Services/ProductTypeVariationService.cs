using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.ProductTypeVariation;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace HRIS.Infrastructure.Shared.Services
{
    public class ProductTypeVariationService : IProductTypeVariationService
    {
        private readonly ILogger<ProductTypeVariationService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;

        public ProductTypeVariationService(ILogger<ProductTypeVariationService> logger,
                                     IAuthenticatedUserService authenticatedUser,
                                     IDapper dapper)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
        }
        public int CreateProductTypeVariation(CreateProductTypeVariationDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Description", model.Description);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                param.Add("BranchId", model.BranchId);

                var response = _dapper.Execute(ApplicationConstant.Sp_ProductTypeVariation, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving ProductTypeVariation");
                return 0;
            }
        }

        public int DeleteProductTypeVariation(DeleteProductTypeVariationDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("ProductTypeVariationId", model.ProductTypeVariationId);
                param.Add("DeletedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_ProductTypeVariation, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting a category");
                return 0;
            }
        }

        public List<ProductTypeVariationDTO> GetAllProductTypeVariation(int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", branchId);
                var response = _dapper.GetAll<ProductTypeVariationDTO>(ApplicationConstant.Sp_ProductTypeVariation, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a list of ProductTypeVariation");
                return new List<ProductTypeVariationDTO>();
            }
        }

        public ProductTypeVariationDTO GetProductTypeVariationById(int ProductTypeVariationId,int branchId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("ProductTypeVariationId", ProductTypeVariationId);
                param.Add("BranchId", branchId);
                var response = _dapper.Get<ProductTypeVariationDTO>(ApplicationConstant.Sp_ProductTypeVariation, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a ProductTypeVariation");
                return new ProductTypeVariationDTO();
            }
        }

        public int UpdateProductTypeVariation(UpdateProductTypeVariationDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("ProductTypeVariationId", model.ProductTypeVariationId);
                param.Add("Description", model.Description);
                param.Add("UpdatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_ProductTypeVariation, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a ProductTypeVariation");
                return 0;
            }
        }

    }
}
