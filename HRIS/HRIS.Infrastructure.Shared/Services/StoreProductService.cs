using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.StoreProduct;
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
    public class StoreProductService : IStoreProductService
    {
        private readonly ILogger<StoreProductService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        //  private readonly IOptions<ConnectionStrings> _connectionString;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;

        public StoreProductService(ILogger<StoreProductService> logger,
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

        int IStoreProductService.CreateStoreProduct(CreateStoreProductDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("CategoryId", model.CategoryId);
                if (model.SubCategoryId != null)
                    param.Add("SubCategoryId", int.Parse(model.SubCategoryId));
                param.Add("ProductName", model.ProductName);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_StoreProduct, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving Store Product");
                return 0;
            }
        }

        int IStoreProductService.DeleteStoreProduct(int StoreProductID)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("StoreProductID", StoreProductID);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_StoreProduct, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting Store Product");
                return 0;
            }
        }

        List<StoreProductDTO> IStoreProductService.GetAllStoreProducts()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                var response = _dapper.GetAll<StoreProductDTO>(ApplicationConstant.Sp_StoreProduct, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating  Store Product");
                return null;
            }
        }

        StoreProductDTO IStoreProductService.GetStoreProductByID(int StoreProductID)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("StoreProductID", StoreProductID);
                var response = _dapper.Get<StoreProductDTO>(ApplicationConstant.Sp_StoreProduct, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while fetching a Store Product");
                return new StoreProductDTO();
            }
        }

        int IStoreProductService.UpdateStoreProduct(UpdateStoreProductDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("StoreProductId", model.StoreProductId);
                param.Add("ProductName", model.ProductName);
                param.Add("CategoryId", model.CategoryId);
                param.Add("SubCategoryId", int.Parse(model.SubCategoryId));
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_StoreProduct, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store Product");
                return 0;
            }
        }
    }

}
