using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs;
using HRIS.Application.DTOs.Store;
using HRIS.Application.Enums;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{
    public class StoreSetupRepository : IStoreSetupRepository
    {

        private readonly ILogger<StoreSetupRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly IAuditRepository _auditRepository;

        public StoreSetupRepository(ILogger<StoreSetupRepository> logger,
                             IAuthenticatedUserService authenticatedUser,
                             IOptions<ConnectionStrings> connectionString,
                             IDapper dapper,
                             UserManager<ApplicationUser> userManager,
                             IAccountService accountService,
                             IEmailService emailService,
                             ApplicationDbContext context,
                             IAuditRepository auditRepository)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            _userManager = userManager;
            _accountService = accountService;
            _emailService = emailService;
            _context = context;
            _auditRepository = auditRepository;
            constring = myconnectionString.Value.DefaultConnection;
        }
        public async Task<int> CreateStore(StoreSetUpDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("IsDeleted", model.IsDeleted);
                param.Add("StoreAddress", model.storeAddress);
                param.Add("StoreEmail", model.storeEmail);
                param.Add("StoreName", model.storeName);
                param.Add("StorePhone", model.storePhone);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_StoreSetup, param, CommandType.StoredProcedure);
                if (response > 0)
                {
                    var newStoreAdmin = new ApplicationUser
                    {
                        FirstName = model.storeName,
                        LastName = "",
                        Email = model.storeEmail,
                        UserName = model.storeEmail,
                        PhoneNumber = model.storePhone,
                        EmailConfirmed = false,
                        IsActive = true
                    };
                    var result = await _userManager.CreateAsync(newStoreAdmin);
                    await _userManager.AddToRoleAsync(newStoreAdmin, "StoreAdmin");

                    try
                    {
                        var verificationUrl = await SendVerificationEmail(newStoreAdmin, model.WebsiteLocation);

                        var msgBody = EmailHelper.ConfirmEmailTemplate(model.storeEmail, verificationUrl);
                        _emailService.SendEmail(model.storeEmail, model.storeName, "Account Confirmation", msgBody);
                    }
                    catch (Exception ex)
                    {

                        response = -12;
                        //var store = await _context.StoreSetup.FirstOrDefaultAsync(c => c.StoreEmail == model.storeEmail);
                        //store.EmailSent = true;
                    }
                    finally
                    {
                        await AssignPagesToStoreAdminUser(newStoreAdmin.Id);
                    }
                    //ToDO



                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving Store");
                return 0;
            }
        }
        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "auth/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            //Email Service Call Here
            return verificationUri;
        }
        //public int DeleteExpenditure(int StoreID)
        //{
        //    try
        //    {
        //        var param = new DynamicParameters();
        //        param.Add("Status", Status.DELETE);
        //        param.Add("StoreID", StoreID);
        //        param.Add("DeletedBy", _authenticatedUser.UserId);
        //        var response = _dapper.Execute(ApplicationConstant.Sp_Product, param, CommandType.StoredProcedure);
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occured while deleting Store");
        //        return 0;
        //    }
        //}

        public List<StoreSetUpDTO> GetAllStores()
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.GetAll<StoreSetUpDTO>(ApplicationConstant.Sp_StoreSetup, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return null;
            }
        }

        public StoreSetUpDTO GetStoreByID(int StoreID)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("StoreID", StoreID);
                var response = _dapper.Get<StoreSetUpDTO>(ApplicationConstant.Sp_StoreSetup, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return new StoreSetUpDTO();
            }
        }

        public int UpdateStore(StoreUpdateDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("StoreSetupId", model.StoreSetupId);
                param.Add("StoreAddress", model.StoreAddress);
                param.Add("StoreEmail", model.StoreEmail);
                param.Add("StoreName", model.StoreName);
                param.Add("StorePhone", model.StorePhone);
                param.Add("UpdatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_StoreSetup, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a Store");
                return 0;
            }
        }

        public async Task AssignPagesToStoreAdminUser(string userId)
        {
            var menuIds = ApplicationConstant.StoreAdminPages();
            foreach (var menu in menuIds)
            {
                var menuIdentity = await _context.MenuSetup.FirstOrDefaultAsync(c => c.MenuId == menu);
                var IsMenuAssignedPreviously = await _context.UsersRolePermission.AnyAsync(c => c.MenuIdentity == menuIdentity.MenuIdentity && c.UserId == userId);
                if (!IsMenuAssignedPreviously)
                {
                    var userRolePermission = new UsersRolePermission
                    {
                        MenuIdentity = menuIdentity.MenuIdentity,
                        UserId = userId,
                        IsActive = true,
                        CreatedOn = DateTime.Now,
                        CreatedBy = userId
                    };
                    await _context.UsersRolePermission.AddAsync(userRolePermission);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        string action = $"Assigned {menuIdentity.MenuName} to {userId}";
                        await _auditRepository.CreateAudit(userId, action);
                    }
                }
                else
                {
                }
            }
        }
    }
}
