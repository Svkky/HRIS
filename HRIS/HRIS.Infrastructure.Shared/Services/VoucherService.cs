using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Voucher;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
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
    public class VoucherService : IVoucherService
    {
        private readonly ILogger<VoucherService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        private readonly ApplicationDbContext _context;

        public VoucherService(ILogger<VoucherService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IOptions<ConnectionStrings> connectionString,
                               IDapper dapper,
                               ApplicationDbContext context)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            _context = context;
            constring = myconnectionString.Value.DefaultConnection;
        }

        public int CreateVoucher(CreateVoucherDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("Description", model.description);
                param.Add("amount", model.amount);
                param.Add("ExpiredDate", model.expiryDate);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                param.Add("BranchId", model.branchId);
                var response = _dapper.Execute(ApplicationConstant.SP_Voucher, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving voucher");
                return 0;
            }
        }

        public int UpdateVoucher(UpdateVoucherDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("VoucherID", model.voucherId);
                param.Add("Description", model.description);
                param.Add("amount", model.amount);
                param.Add("ExpiredDate", model.expiryDate);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.SP_Voucher, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a voucher");
                return 0;
            }
        }

        public int DeleteVoucher(DeleteVoucherDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("VoucherID", model.voucherId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.SP_Voucher, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting voucher");
                return 0;
            }
        }

        public List<VoucherDTO> GetAllVoucher(int branchId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchId", branchId);
                var response = _dapper.GetAll<VoucherDTO>(ApplicationConstant.SP_Voucher, param, commandType: CommandType.StoredProcedure);
                response.ForEach(c => c.amt = String.Format("{0:n0}", c.amount));
                response.ForEach(c => c.expiry = c.expiryDate.ToString("dd/MM/yyyy"));
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while returning voucher");
                return null;
            }
        }

        public VoucherDTO GetVoucherById(int VoucherId, int branchId)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("VoucherID", VoucherId);
                param.Add("BranchId", branchId);
                var response = _dapper.Get<VoucherDTO>(ApplicationConstant.SP_Voucher, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting Voucher");
                return new VoucherDTO();
            }
        }

        public async Task<VoucherDTO> GetVoucherByVoucherNo(string voucherNo, string customerPhone)
        {
            try
            {
                var now = DateTime.Now;
                var isVoucherActive = await _context.Voucher.AnyAsync(c => c.VoucherNo == voucherNo && c.IsDeleted == false && c.ExpiryDate.Date >= now.Date);
                if (!isVoucherActive)
                {
                    return null;
                }
                var customer = await _context.Customer.FirstOrDefaultAsync(c => c.Phone == customerPhone);
                if (customer is null)
                {
                    return null;
                }
                var isVoucherBelongsToCustomerAndActive = await _context.CustomerVoucher.AnyAsync(c => c.IsDeleted == false && c.VoucherNo == voucherNo && c.CustomerId == customer.CustomerId && c.Balance > 0);
                if (!isVoucherBelongsToCustomerAndActive)
                {
                    return null;
                }
                var voucherDTO = await _context.CustomerVoucher
                    .Include(c => c.Voucher)
                    .Include(c => c.Customer)
                    .Where(c => c.CustomerId == customer.CustomerId
                           && c.VoucherNo == voucherNo
                           && c.Voucher.IsDeleted == false
                           && c.IsDeleted == false)
                    .Select(c => new VoucherDTO
                    {
                        voucherNo = c.VoucherNo,
                        voucherId = c.VoucherId,
                        amount = c.Amount,
                        description = c.Voucher.Description,
                        expiryDate = c.Voucher.ExpiryDate,
                        balance = c.Balance,
                        isActive = c.IsDeleted
                    }).FirstOrDefaultAsync();
                return voucherDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting Voucher");
                return null;
            }
        }
    }
}
