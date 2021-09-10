using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Sales;
using HRIS.Application.DTOs.Warehouse;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
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
    public class SalesService : ISalesService
    {
        private readonly IDapper dapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<SalesService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;

        public SalesService(ILogger<SalesService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IOptions<ConnectionStrings> connectionString,
                               IDapper dapper,
                               ApplicationDbContext context,
                               UserManager<ApplicationUser> userManager)
        {
            this.dapper = dapper;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            constring = myconnectionString.Value.DefaultConnection;
        }

        public SalesVm CreateSales(ParentSales model)
        {
            var d = DateTime.Now;
            string year = d.Year.ToString().Substring(2, 2);
            string month = d.Month.ToString().Length > 1 ? d.Month.ToString() : '0' + d.Month.ToString();
            string day = d.Day.ToString().Length > 1 ? d.Day.ToString() : '0' + d.Day.ToString();

            string lol = year + month + day;
            var pol = new SalesVm();

            var billNumber = generate_Digits(8) + lol + DateTime.Now.ToString("fff");
            int response2 = 0;
            try
            {
                var total = (model.detail.Sum(c => int.Parse(c.subTotal)) - int.Parse(model.sales.TotalDiscount));
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("BillFromCode", billNumber);
                param.Add("CustomerName", model.sales.CustomerName);
                param.Add("TotalAmount", total);
                param.Add("Change", Math.Abs((total - int.Parse(model.sales.TotalPaid))));
                param.Add("CustomerPhone", model.sales.CustomerPhone);
                param.Add("isVoucherUsed", model.sales.isVoucherUsed);
                param.Add("ModeOfPayment", model.sales.ModeOfPayment);
                param.Add("CustomerVoucherId", model.sales.CustomerVoucherId);
                param.Add("isPaid", model.sales.isPaid);
                param.Add("TotalDiscount", double.Parse(model.sales.TotalDiscount));
                param.Add("TotalVat", double.Parse(model.sales.TotalVat));
                param.Add("DatePaid", model.sales.DatePaid);
                param.Add("TotalPaid", double.Parse(model.sales.TotalPaid));
                param.Add("CreatedBy", _authenticatedUser.UserId);
                param.Add("BranchId", model.sales.branchId);


                var response = dapper.Execute(ApplicationConstant.SP_Sales, param, CommandType.StoredProcedure);


                if (response > 0)
                {
                    foreach (var k in model.detail)
                    {
                        var param2 = new DynamicParameters();
                        param2.Add("Status", Status.INSERT);
                        param2.Add("BillNumber", billNumber);
                        param2.Add("productId", int.Parse(k.productId));
                        param2.Add("Quantity ", k.quantity);
                        param2.Add("SellingPrice", k.sellingPrice);
                        param2.Add("SubTotal", double.Parse(k.subTotal));
                        param2.Add("Discount", k.discount);
                        param2.Add("VatPercent ", k.vatPercent);
                        param2.Add("BranchId", k.branchId);
                        param2.Add("vatValue", double.Parse(k.vatValue));
                        param2.Add("CreatedBy", _authenticatedUser.UserId);

                        response2 = dapper.Execute(ApplicationConstant.SP_SalesDetail, param2, CommandType.StoredProcedure);
                    }
                }


                if (response2 > 0)
                {
                    pol = GetAllSales(billNumber);
                }
                else
                {
                    pol = new SalesVm();
                }

                return pol;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving category");
                return new SalesVm();
            }
        }
        public async Task<SalesVm> AddSaleAsync(ParentSales model)
        {

            try
            {
                var newSale = CreateSalesObj(model);
                await _context.Sales.AddAsync(newSale);
                var isSaved = await _context.SaveChangesAsync();

                if (isSaved > 0)
                {
                    var salesDetailList = CreateSalesDetailsObj(model, newSale.BillNumber);
                    await _context.SalesDetail.AddRangeAsync(salesDetailList);
                    var saved = await _context.SaveChangesAsync();

                    await UpdateBranchProducts(salesDetailList);

                    var saleRecord = new SalesVm();
                    saleRecord = await GetAllSaleAsync(newSale.BillNumber);
                    return saleRecord;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task UpdateBranchProducts(List<SalesDetail> salesDetails)
        {
            foreach (var item in salesDetails)
            {

                var branchProduct = await _context.BranchProducts.FirstOrDefaultAsync(c => c.StoreProductId == item.StoreProductId);
                branchProduct.Quantity -= item.Quantity;
                _context.BranchProducts.Update(branchProduct);
            }
            await _context.SaveChangesAsync();
        }
        public List<SalesDetail> CreateSalesDetailsObj(ParentSales request, string billNumber)
        {
            var salesDetailList = new List<SalesDetail>();
            foreach (var detail in request.detail)
            {
                salesDetailList.Add(new SalesDetail
                {
                    BillNumber = billNumber,
                    BranchId = request.sales.branchId,
                    CreatedBy = _authenticatedUser.UserId,
                    CreatedOn = DateTime.Now,
                    DateOfPurchase = DateTime.Now,
                    Discount = detail.discount,
                    StoreProductId = _context.BranchProducts.FirstOrDefault(c => c.BranchProductId == int.Parse(detail.productId)).StoreProductId,
                    Quantity = detail.quantity,
                    SellingPrice = detail.sellingPrice,
                    SubTotal = double.Parse(detail.subTotal),
                    IsDeleted = false,
                    VatValue = double.Parse(detail.vatValue),
                    VatPercent = detail.vatPercent,
                    IsVatPaid = false
                });
            }
            return salesDetailList;
        }
        public Sales CreateSalesObj(ParentSales request)
        {
            var total = (request.detail.Sum(c => int.Parse(c.subTotal)) - int.Parse(request.sales.TotalDiscount));
            var sales = new Sales
            {
                BillNumber = GenerateBillNumber(),
                BranchId = request.sales.branchId,
                TotalAmount = total,
                TotalDiscount = double.Parse(request.sales.TotalDiscount),
                Change = Math.Abs((total - int.Parse(request.sales.TotalPaid))),
                TotalPaid = double.Parse(request.sales.TotalPaid),
                CreatedOn = DateTime.Now,
                CreatedBy = _authenticatedUser.UserId,
                DatePaid = DateTime.Now,
                IsDeleted = false,
                IsPaid = request.sales.isPaid,
                ModeOfPayment = request.sales.ModeOfPayment,
                TotalVat = double.Parse(request.sales.TotalVat),
                IsVoucherUsed = request.sales.isVoucherUsed,
                TotalBalance = int.Parse(request.sales.TotalPaid) > total ? 0 : (total - int.Parse(request.sales.TotalPaid)),
                RecieptNumber = request.sales.isPaid ? GenerateReceiptNumber() : null
            };
            if (request.sales.CustomerPhone != "")
                sales.CustomerId = GetCustomerID(request.sales.CustomerPhone, request.sales.CustomerName, request.sales.branchId);
            return sales;
        }

        public int GetCustomerID(string phone, string Name, int BranchId)
        {
            var customer = _context.Customer.FirstOrDefault(c => c.Phone == phone);
            if (customer != null)
                return customer.CustomerId;
            var newCustomer = new Customer
            {
                BranchId = BranchId,
                FullName = Name,
                Phone = phone,
                CreatedBy = _authenticatedUser.UserId,
                IsDeleted = false,
                CreatedOn = DateTime.Now
            };

            _context.Customer.Add(newCustomer);
            var saved = _context.SaveChanges();
            return newCustomer.CustomerId;

        }
        public string GenerateBillNumber()
        {
            var d = DateTime.Now;
            string year = d.Year.ToString().Substring(2, 2);
            string month = d.Month.ToString().Length > 1 ? d.Month.ToString() : '0' + d.Month.ToString();
            string day = d.Day.ToString().Length > 1 ? d.Day.ToString() : '0' + d.Day.ToString();

            string lol = year + month + day;
            var pol = new SalesVm();

            var billNumber = generate_Digits(8) + lol + DateTime.Now.ToString("fff");
            return billNumber;
        }
        public string GenerateReceiptNumber()
        {
            var d = DateTime.Now;
            string year = d.Year.ToString().Substring(2, 2);
            string month = d.Month.ToString().Length > 1 ? d.Month.ToString() : '0' + d.Month.ToString();
            string day = d.Day.ToString().Length > 1 ? d.Day.ToString() : '0' + d.Day.ToString();

            string lol = year + month + day;

            var billNumber = "REC" + generate_Digits(10) + lol + DateTime.Now.ToString("fff");
            return billNumber;
        }

        private static string generate_Digits(int length)
        {
            var rndDigits = new System.Text.StringBuilder().Insert(0, "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ", length).ToString().ToCharArray();
            return string.Join("", rndDigits.OrderBy(o => Guid.NewGuid()).Take(length));
        }

        public SalesVm GetAllSales(string BillNumber)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BillNumber", BillNumber);
                param.Add("BranchId", _authenticatedUser.BranchId);


                var response = dapper.GetAll<SalesDTO>(ApplicationConstant.SP_Sales, param, commandType: CommandType.StoredProcedure);


                var salesVm = new SalesVm
                {
                    TotalDiscount = String.Format("{0:n0}", response.FirstOrDefault().TotalDiscount),
                    TotalPaid = String.Format("{0:n0}", response.FirstOrDefault().TotalPaid),
                    TotalVat = String.Format("{0:n0}", response.FirstOrDefault().TotalVat),
                    BillNumber = response.FirstOrDefault().BillNumber,
                    ReceiptNumber = response.FirstOrDefault().ReceiptNumber,
                    Fullname = response.FirstOrDefault().Fullname,
                    PhoneNumber = response.FirstOrDefault().PhoneNumber,
                    DatePaid = response.FirstOrDefault().DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                    BranchName = response.FirstOrDefault().BranchName,
                    StoreName = response.FirstOrDefault().StoreName,
                    Change = String.Format("{0:n0}", response.FirstOrDefault().Change),
                    TotalAmount = String.Format("{0:n0}", response.FirstOrDefault().TotalAmount),
                };

                var item = new List<SalesDetailVm>();
                response.ForEach(x =>
                {
                    item.Add(new SalesDetailVm
                    {
                        Name = x.Name,
                        Quantity = x.Quantity,
                        SubTotal = x.SubTotal
                    });

                });

                salesVm.SalesDetailVm = item;


                return salesVm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }
        public async Task<SalesVm> GetAllSaleAsync(string BillNumber)
        {
            try
            {

                var sales = await _context.Sales.Include(c => c.Customer).Include(c => c.Branch).FirstOrDefaultAsync(c => c.BillNumber == BillNumber);
                var salesDetails = await _context.SalesDetail.Include(c => c.StoreProduct).Where(c => c.BillNumber == BillNumber).ToListAsync();

                var salesVm = new SalesVm
                {
                    TotalDiscount = String.Format("{0:n0}", sales.TotalDiscount),
                    TotalPaid = String.Format("{0:n0}", sales.TotalPaid),
                    TotalVat = String.Format("{0:n0}", sales.TotalVat),
                    BillNumber = sales.BillNumber,
                    ReceiptNumber = sales.RecieptNumber,
                    Fullname = sales.Customer == null ? "" : sales.Customer.FullName,
                    PhoneNumber = sales.Customer == null ? "" : sales.Customer.Phone,
                    DatePaid = sales.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                    BranchName = sales.Branch.BranchName,
                    StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                    Change = String.Format("{0:n0}", sales.Change),
                    TotalAmount = String.Format("{0:n0}", sales.TotalAmount),
                };

                var item = new List<SalesDetailVm>();
                salesDetails.ForEach(c => item.Add(new SalesDetailVm
                {
                    Name = c.StoreProduct != null ? c.StoreProduct.ProductName : "N/A",
                    Quantity = c.Quantity,
                    SubTotal = c.SubTotal
                }));

                salesVm.SalesDetailVm = item;


                return salesVm;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }
        public async Task<List<SalesVm>> GetSales(string loggedInUserId, string brnchId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(loggedInUserId);
                if (user is null)
                {
                    return null;
                }
                var sales = new List<SalesVm>();
                if (await _userManager.IsInRoleAsync(user, "Teller"))
                {
                    sales = await _context.Sales.Include(c => c.Customer)
                                  .Include(c => c.Branch)
                                  .Where(c => c.CreatedBy == user.Id)
                                  .Select(c => new SalesVm
                                  {
                                      TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                                      TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                      TotalVat = String.Format("{0:n0}", c.TotalVat),
                                      BillNumber = c.BillNumber,
                                      ReceiptNumber = c.RecieptNumber,
                                      Fullname = c.Customer == null ? "" : c.Customer.FullName,
                                      PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                                      DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                                      BranchName = c.Branch.BranchName,
                                      StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                                      ModeOfPayment = c.ModeOfPayment,
                                      Change = String.Format("{0:n0}", c.Change),
                                      TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                                  })
                                  .ToListAsync();
                }
                else
                {
                    if (brnchId != "")
                    {
                        sales = await _context.Sales.Include(c => c.Customer)
                               .Include(c => c.Branch)
                               .Where(c => c.BranchId == int.Parse(brnchId))
                               .Select(c => new SalesVm
                               {
                                   TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                                   TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                   TotalVat = String.Format("{0:n0}", c.TotalVat),
                                   BillNumber = c.BillNumber,
                                   ReceiptNumber = c.RecieptNumber,
                                   Fullname = c.Customer == null ? "" : c.Customer.FullName,
                                   PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                                   DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                                   BranchName = c.Branch.BranchName,
                                   StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                                   ModeOfPayment = c.ModeOfPayment,
                                   Change = String.Format("{0:n0}", c.Change),
                                   TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                               })
                               .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales.Include(c => c.Customer)
                               .Include(c => c.Branch)
                               .Where(c => c.BranchId == int.Parse(_authenticatedUser.BranchId))
                               .Select(c => new SalesVm
                               {
                                   TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                                   TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                   TotalVat = String.Format("{0:n0}", c.TotalVat),
                                   BillNumber = c.BillNumber,
                                   ReceiptNumber = c.RecieptNumber,
                                   Fullname = c.Customer == null ? "" : c.Customer.FullName,
                                   PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                                   DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                                   BranchName = c.Branch.BranchName,
                                   StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                                   ModeOfPayment = c.ModeOfPayment,
                                   Change = String.Format("{0:n0}", c.Change),
                                   TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                               })
                               .ToListAsync();
                    }


                }


                return sales;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }
        public async Task<List<SalesVm>> FilterSales(SearchFilter searchFilter, string loggedInUserId, string brnchId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(loggedInUserId);
                if (user is null)
                {
                    return null;
                }
                var sales = new List<SalesVm>();
                if (await _userManager.IsInRoleAsync(user, "Teller"))
                {
                    sales = await _context.Sales.Include(c => c.Customer)
                            .Include(c => c.Branch)
                            .Where(c => c.CreatedBy == loggedInUserId
                             && (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To)
                             && (c.ModeOfPayment == searchFilter.TransactionType || searchFilter.TransactionType == null))
                            .OrderByDescending(c => c.DatePaid)
                            .Select(c => new SalesVm
                            {
                                TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                                TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                                TotalVat = String.Format("{0:n0}", c.TotalVat),
                                BillNumber = c.BillNumber,
                                ReceiptNumber = c.RecieptNumber,
                                Fullname = c.Customer == null ? "" : c.Customer.FullName,
                                PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                                DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                                BranchName = c.Branch.BranchName,
                                StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                                ModeOfPayment = c.ModeOfPayment,
                                Change = String.Format("{0:n0}", c.Change),
                                TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                            })
                            .ToListAsync();
                }
                else
                {
                    if (brnchId != null)
                    {
                        sales = await _context.Sales.Include(c => c.Customer)
                           .Include(c => c.Branch)
                           .Where(c => c.BranchId == int.Parse(brnchId)
                            && (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To)
                            && (c.ModeOfPayment == searchFilter.TransactionType || searchFilter.TransactionType == null))
                           .OrderByDescending(c => c.DatePaid)
                           .Select(c => new SalesVm
                           {
                               TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                               TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                               TotalVat = String.Format("{0:n0}", c.TotalVat),
                               BillNumber = c.BillNumber,
                               ReceiptNumber = c.RecieptNumber,
                               Fullname = c.Customer == null ? "" : c.Customer.FullName,
                               PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                               DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                               BranchName = c.Branch.BranchName,
                               StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                               ModeOfPayment = c.ModeOfPayment,
                               Change = String.Format("{0:n0}", c.Change),
                               TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                           })
                           .ToListAsync();
                    }
                    else
                    {
                        sales = await _context.Sales.Include(c => c.Customer)
                           .Include(c => c.Branch)
                           .Where(c => (c.CreatedOn >= searchFilter.From && c.CreatedOn <= searchFilter.To)
                            && (c.ModeOfPayment == searchFilter.TransactionType || searchFilter.TransactionType == null))
                           .OrderByDescending(c => c.DatePaid)
                           .Select(c => new SalesVm
                           {
                               TotalDiscount = String.Format("{0:n0}", c.TotalDiscount),
                               TotalPaid = String.Format("{0:n0}", c.TotalPaid),
                               TotalVat = String.Format("{0:n0}", c.TotalVat),
                               BillNumber = c.BillNumber,
                               ReceiptNumber = c.RecieptNumber,
                               Fullname = c.Customer == null ? "" : c.Customer.FullName,
                               PhoneNumber = c.Customer == null ? "" : c.Customer.Phone,
                               DatePaid = c.DatePaid.ToString("dd/MM/yyyy hh:mm tt"),
                               BranchName = c.Branch.BranchName,
                               StoreName = _context.StoreSetup.FirstOrDefault() != null ? _context.StoreSetup.FirstOrDefault().StoreName : "",
                               ModeOfPayment = c.ModeOfPayment,
                               Change = String.Format("{0:n0}", c.Change),
                               TotalAmount = String.Format("{0:n0}", c.TotalAmount),
                           })
                           .ToListAsync();
                    }


                }

                return sales;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }


        public async Task<List<SalesDetailVM>> GetSalesDetails(string billNumber)
        {
            try
            {
                var salesDetails = await _context.SalesDetail
                                      .Include(c => c.StoreProduct)
                                      .Where(c => c.BillNumber == billNumber)
                                      .Select(c => new SalesDetailVM
                                      {
                                          SaleDetailId = c.SalesDetailId,
                                          Discount = String.Format("{0:n0}", c.Discount),
                                          Name = c.StoreProduct.ProductName,
                                          Quantity = c.Quantity,
                                          SellingPrice = String.Format("{0:n0}", c.SellingPrice),
                                          SubTotal = String.Format("{0:n0}", c.SubTotal),
                                          VatPercent = String.Format("{0:n0}", c.VatPercent),
                                          VatValue = String.Format("{0:n0}", c.VatValue)
                                      })
                                      .ToListAsync();

                return salesDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }

        }

        public async Task<List<BranchSales>> BranchSales()
        {
            try
            {
                var sales = await _context.Sales.Include(c => c.Customer)
                                .Include(c => c.Branch)
                                .GroupBy(c => c.BranchId).Where(c => c.Count() >= 1)
                                .Select(group => new BranchSales
                                {
                                    BranchId = group.Key,
                                    TotalOrders = group.Count(),
                                    TotalAmount = String.Format("{0:n0}", group.Sum(c => c.TotalAmount)),
                                })
                                .ToListAsync();
                sales.ForEach(c => c.BranchName = _context.Branch.FirstOrDefault(y => y.BranchId == c.BranchId).BranchName);
                return sales;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }

    }
}
