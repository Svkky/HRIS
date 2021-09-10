using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Products;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using HRIS.Infrastructure.Interfaces.Repositories;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{

    public class ProductsRepository : IProductsRepository
    {
        private readonly ILogger<ProductsRepository> _logger;
        private readonly IProductTypeVariationService service;
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        public ProductsRepository(ILogger<ProductsRepository> logger,
                             IAuthenticatedUserService authenticatedUser,
                             IOptions<ConnectionStrings> connectionString,
                             IDapper dapper, IProductTypeVariationService service,
                             ApplicationDbContext context)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            constring = myconnectionString.Value.DefaultConnection;
            this.service = service;
            _context = context;
        }
        public async Task<BranchProductDTO> GetProductByID(int ProductID, int branchId)
        {
            try
            {

                var response = await _context.BranchProducts
                    .Where(c => c.BranchId == branchId && c.BranchProductId == ProductID)
                    .Select(c => new BranchProductDTO
                    {
                        BranchId = c.BranchId,
                        BranchProductId = c.BranchProductId,
                        canExpire = c.CanExpire,
                        CriticalLevel = c.CriticalLevel,
                        Discount = c.Discount,
                        ExpiryDate = c.ExpiryDate,
                        ManufactureDate = c.ManufactureDate,
                        ProductName = c.StoreProduct.ProductName,
                        ProductTypeVariation = c.ProductVariation.Description,
                        ProductTypeVariationId = c.ProductTypeVariationId.ToString(),
                        QuantityRemaning = String.Format("{0:n0}", c.Quantity),
                        SellingPrice = String.Format("{0:n0}", c.SellingPrice),
                        VariationQuantity = c.VariationQuantity,
                        VatPercent = c.VatPercent,
                        IsConfigured = c.IsConfigured,
                        CreatedOn = c.CreatedOn.ToString("dd/MM/yyyy hh:mm tt")
                    })
                    .FirstOrDefaultAsync();
                //var response = _dapper.Get<BranchProductDTO>(ApplicationConstant.Sp_Product, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return new BranchProductDTO();
            }
        }


        public async Task<List<BranchProductDTO>> GetAllProducts(int branchId)
        {
            try
            {
                var result = await _context.BranchProducts
                    .Include(c => c.ProductVariation)
                    .Include(c => c.StoreProduct)
                    .Where(c => c.BranchId == branchId)
                    .Select(c => new BranchProductDTO
                    {
                        BranchId = c.BranchId,
                        BranchProductId = c.BranchProductId,
                        canExpire = c.CanExpire,
                        CriticalLevel = c.CriticalLevel,
                        Discount = c.Discount,
                        ExpiryDate = c.ExpiryDate,
                        ManufactureDate = c.ManufactureDate,
                        ProductName = c.StoreProduct.ProductName,
                        ProductTypeVariation = c.ProductVariation.Description,
                        ProductTypeVariationId = c.ProductTypeVariationId.ToString(),
                        QuantityRemaning = String.Format("{0:n0}", c.Quantity),
                        SellingPrice = String.Format("{0:n0}", c.SellingPrice),
                        VariationQuantity = c.VariationQuantity,
                        VatPercent = c.VatPercent,
                        IsConfigured = c.IsConfigured,
                        CreatedOn = c.CreatedOn.ToString("dd/MM/yyyy hh:mm tt")
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return null;
            }
        }

        public async Task<List<ProductAllocationDTO>> GetBranchProducts(int branchId)
        {
            try
            {
                var result = await _context.ProductAllocationUse
                    .Include(c => c.StoreProduct)
                    .Where(c => c.BranchId == branchId)
                    .Select(c => new ProductAllocationDTO
                    {
                        ProductAllocationUseId = c.ProductAllocationUseId,
                        QuantityRemaining = c.AllocationQuantityRemaining,
                        ProductName = c.StoreProduct.ProductName
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return null;
            }
        }



        //public BranchProductDTO GetProductByID(int ProductID, int branchId)
        //{
        //    try
        //    {


        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occured while updating a category");
        //        return new ProductDTO();
        //    }
        //}

        public async Task<int> UpdateProductAsync(EditBranchProductDTO model)
        {
            try
            {
                var branchProduct = await _context.BranchProducts.FirstOrDefaultAsync(c => c.BranchProductId == int.Parse(model.BranchProductId));
                if (branchProduct is null)
                {
                    return -1;
                }

                branchProduct.CanExpire = model.CanExpire;
                branchProduct.CriticalLevel = int.Parse(model.CriticalLevel);
                branchProduct.Discount = double.Parse(model.Discount);
                branchProduct.ExpiryDate = model.ExpiryDate;
                branchProduct.ManufactureDate = model.ManufactureDate;
                if (model.ProductTypeVariationId != null)
                    branchProduct.ProductTypeVariationId = int.Parse(model.ProductTypeVariationId);

                branchProduct.SellingPrice = model.SellingPrice;
                branchProduct.VariationQuantity = model.VariationQuantity;
                branchProduct.VatPercent = int.Parse(model.VatPercent);
                branchProduct.IsConfigured = true;
                branchProduct.UpdatedBy = _authenticatedUser.UserId;
                branchProduct.UpdatedOn = DateTime.Now;

                _context.BranchProducts.Update(branchProduct);
                var result = await _context.SaveChangesAsync();
                return result;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a category");
                return 0;
            }
        }
    }
}
