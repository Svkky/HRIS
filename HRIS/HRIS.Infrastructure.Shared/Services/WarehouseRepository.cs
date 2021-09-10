using HRIS.Application.DTOs.Warehouse;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private static Random random = new Random();
        public WarehouseRepository(ApplicationDbContext context,
                                   IAuthenticatedUserService authenticatedUserService,
                                   UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _authenticatedUserService = authenticatedUserService;
            _userManager = userManager;
        }
        public async Task<int> AddProductToWarehouse(AddProductToWareHouseDTO request)
        {
            //ToDOs
            /*
              Generate a unique bill number
              Save to VendorPaymentMaster 
              Update product warehouse 
            */
            var IsSaved = await SaveVendorPayment(request);
            if (IsSaved)
            {
                await UpdateProductWarehouse(request);
                return 1;
            }
            return 0;
        }

        private async Task UpdateProductWarehouse(AddProductToWareHouseDTO request)
        {
            var product = await _context.ProductWareHouse.OrderByDescending(c => c.ProductWareHouseId).FirstOrDefaultAsync(c => c.StoreProductId == request.StoreProductId);
            if (product != null)
            {
                var newproduct = new ProductWareHouse
                {
                    BalanceBefore = product.BalanceAfter,
                    Quantity = request.TotalQuantity,
                    BalanceAfter = product.BalanceAfter + request.TotalQuantity,
                    StoreProductId = request.StoreProductId,
                    TransactionType = "Inward",
                    CreatedBy = _authenticatedUserService.UserId,
                    CreatedOn = DateTime.Now
                };
                await _context.ProductWareHouse.AddAsync(newproduct);
            }
            else
            {
                var newproduct = new ProductWareHouse
                {
                    BalanceBefore = 0,
                    Quantity = request.TotalQuantity,
                    BalanceAfter = request.TotalQuantity,
                    StoreProductId = request.StoreProductId,
                    TransactionType = "Inward",
                    CreatedBy = _authenticatedUserService.UserId,
                    CreatedOn = DateTime.Now,
                };
                await _context.ProductWareHouse.AddAsync(newproduct);

            }
            await _context.SaveChangesAsync();
        }

        private static string BillNumberGenerator(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private async Task<bool> SaveVendorPayment(AddProductToWareHouseDTO request)
        {
            var vendorPayment = new VendorPaymentMaster
            {
                SupplierId = request.VendorId,
                TotalAmount = request.TotalAmount,
                TotalItemsPerCarton = request.TotalItemsPerCarton,
                Balance = request.Balance,
                BillNo = BillNumberGenerator(18),
                Cartons = request.Cartons,
                DeliveryDate = request.DeliveryDate,
                StoreProductId = request.StoreProductId,
                TotalQuantity = request.TotalQuantity,
                TotalPaid = request.TotalPaid,
                InvoiceDocument = request.InvoiceDocument,
                CreatedOn = DateTime.Now,
                CreatedBy = _authenticatedUserService.UserId
            };
            await _context.VendorPaymentMaster.AddAsync(vendorPayment);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return true;
            else return false;
        }

        public async Task<List<VendorPaymentDTO>> GetVendorPayments()
        {
            try
            {
                var payments = await _context.VendorPaymentMaster
                    .Include(c => c.StoreProduct)
                    .Include(c => c.Supplier)
                    .Select(c => new VendorPaymentDTO
                    {
                        Balance = String.Format("{0:n0}", c.Balance),
                        BillNo = c.BillNo,
                        Cartons = c.Cartons,
                        CreatedBy = c.CreatedBy,
                        CreatedOn = c.CreatedOn,
                        DeliveryDate = c.DeliveryDate,
                        StoreProductId = c.StoreProductId,
                        TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                        TotalItemsPerCarton = c.TotalItemsPerCarton,
                        TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                        TotalQuantity = c.TotalQuantity,
                        VendorId = c.SupplierId,
                        ProductName = c.StoreProduct.ProductName,
                        VendorName = c.Supplier.Name,
                        VendorPaymentMasterId = c.VendorPaymentMasterId
                    }).ToListAsync();
                return payments;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ProductWareHouseDTO>> GetWareHouseProduct()
        {
            try
            {
                var result = await _context.ProductWareHouse
                    .Include(c => c.StoreProduct)
                    .Include(c => c.Branch)
                    .OrderBy(c => c.StoreProductId)
                    .OrderBy(c => c.ProductWareHouseId)
                    .Select(c => new ProductWareHouseDTO
                    {
                        CreatedOn = c.CreatedOn.ToString("dd/MM/yyyy hh:mm tt"),
                        DateCreated = c.CreatedOn,
                        AllocatedQuantity = String.Format("{0:n0}", c.Quantity),
                        BalanceAfter = String.Format("{0:n0}", c.BalanceAfter),
                        BalanceBefore = String.Format("{0:n0}", c.BalanceBefore),
                        TransactionType = c.TransactionType,
                        ProductId = c.StoreProductId,
                        ProductWareHouseId = c.ProductWareHouseId,
                        ProductName = c.StoreProduct.ProductName,
                        BranchName = c.Branch == null ? "N/A" : c.Branch.BranchName,
                        CreatedBy = c.CreatedBy
                    }).ToListAsync();
                result.ForEach(c => c.CreatedBy = _userManager.Users.Where(x => x.Id == c.CreatedBy).Select(d => d.Email).FirstOrDefault());
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> GetTotalRemainingProduct(int ProductId)
        {
            try
            {
                var productinWarehouse = await _context.ProductWareHouse.OrderByDescending(c => c.ProductWareHouseId).Where(c => c.StoreProductId == ProductId).FirstOrDefaultAsync();
                if (productinWarehouse == null)
                    return -1;
                return productinWarehouse.BalanceAfter;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        public async Task<List<VendorPaymentDTO>> FilterPayments(SearchFilter searchFilter)
        {
            try
            {
                if (searchFilter.SupplierId == "")
                {
                    var response = await _context.VendorPaymentMaster.Where(c =>
                                  (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To))
                                  .Select(c => new VendorPaymentDTO
                                  {
                                      Balance = String.Format("{0:n0}", c.Balance),
                                      BillNo = c.BillNo,
                                      Cartons = c.Cartons,
                                      CreatedBy = c.CreatedBy,
                                      CreatedOn = c.CreatedOn,
                                      DeliveryDate = c.DeliveryDate,
                                      StoreProductId = c.StoreProductId,
                                      TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                                      TotalItemsPerCarton = c.TotalItemsPerCarton,
                                      TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                      TotalQuantity = c.TotalQuantity,
                                      VendorId = c.SupplierId,
                                      ProductName = c.StoreProduct.ProductName,
                                      VendorName = c.Supplier.Name,
                                      VendorPaymentMasterId = c.VendorPaymentMasterId
                                  }).ToListAsync();
                    return response;
                }
                else
                {
                    var response = await _context.VendorPaymentMaster.Where(c =>
                                   (c.SupplierId == null || c.SupplierId == int.Parse(searchFilter.SupplierId))
                                   && (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To))
                                  .Select(c => new VendorPaymentDTO
                                  {
                                      Balance = String.Format("{0:n0}", c.Balance),
                                      BillNo = c.BillNo,
                                      Cartons = c.Cartons,
                                      CreatedBy = c.CreatedBy,
                                      CreatedOn = c.CreatedOn,
                                      DeliveryDate = c.DeliveryDate,
                                      StoreProductId = c.StoreProductId,
                                      TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                                      TotalItemsPerCarton = c.TotalItemsPerCarton,
                                      TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                      TotalQuantity = c.TotalQuantity,
                                      VendorId = c.SupplierId,
                                      ProductName = c.StoreProduct.ProductName,
                                      VendorName = c.Supplier.Name,
                                      VendorPaymentMasterId = c.VendorPaymentMasterId
                                  }).ToListAsync();

                    return response;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<List<ProductWareHouseDTO>> FilterProductsInWarehouse(SearchFilter searchFilter)
        {
            try
            {
                var response = await _context.ProductWareHouse
                                 .Include(c => c.StoreProduct)
                                 .Include(c => c.Branch)
                                 .Where(c => (c.StoreProduct.ProductName == searchFilter.ProductName || searchFilter.ProductName == null)
                                 && (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To)
                                 && (c.TransactionType == searchFilter.TransactionType || searchFilter.TransactionType == null))
                                 .OrderBy(c => c.StoreProductId)
                                 .OrderBy(c => c.ProductWareHouseId)
                                   .Select(c => new ProductWareHouseDTO
                                   {
                                       CreatedOn = c.CreatedOn.ToString("dd/MM/yyyy hh:mm tt"),
                                       DateCreated = c.CreatedOn,
                                       AllocatedQuantity = String.Format("{0:n0}", c.Quantity),
                                       BalanceAfter = String.Format("{0:n0}", c.BalanceAfter),
                                       BalanceBefore = String.Format("{0:n0}", c.BalanceBefore),
                                       TransactionType = c.TransactionType,
                                       ProductId = c.StoreProductId,
                                       ProductWareHouseId = c.ProductWareHouseId,
                                       ProductName = c.StoreProduct.ProductName,
                                       BranchName = c.Branch == null ? "N/A" : c.Branch.BranchName,
                                       CreatedBy = c.CreatedBy
                                   }).ToListAsync();
                response.ForEach(c => c.CreatedBy = _userManager.Users.Where(x => x.Id == c.CreatedBy).Select(d => d.Email).FirstOrDefault());
                return response;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
