using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Base;
using HRIS.Application.DTOs.BranchAdmin;
using HRIS.Application.Enums;
using HRIS.Application.Helpers;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Identity.Models;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{
    public class BranchAdminService : IBranchAdminService
    {
        private readonly ILogger<BranchAdminService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private readonly IDapper _dapper;
        private readonly ApplicationDbContext _context;
        private readonly IAuditRepository _auditRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public BranchAdminService(ILogger<BranchAdminService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IDapper dapper,
                               ApplicationDbContext context,
                               IAuditRepository auditRepository,
                               UserManager<ApplicationUser> userManager,
                               IEmailService emailService)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            _dapper = dapper;
            _context = context;
            _auditRepository = auditRepository;
            _userManager = userManager;
            _emailService = emailService;
        }


        public async Task<int> CreateBranchAdmin(CreateBranchAdminDTO model)
        {
            BaseResponse Resp = new BaseResponse();

            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("BranchID", int.Parse(model.BranchId));
                param.Add("FirstName", model.FirstName);
                param.Add("LastName", model.LastName);
                param.Add("Email", model.Email);
                param.Add("Phone", model.Phone);
                param.Add("CreatedBy", _authenticatedUser.UserId);
                var response = _dapper.Execute(ApplicationConstant.Sp_BranchAdmin, param, CommandType.StoredProcedure);
                if (response > 0)
                {
                    var newStoreAdmin = new ApplicationUser
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Email,
                        PhoneNumber = model.Phone,
                        EmailConfirmed = false,
                        BranchId = int.Parse(model.BranchId),
                        IsActive = true,
                    };
                    var result = await _userManager.CreateAsync(newStoreAdmin);
                    await _userManager.AddToRoleAsync(newStoreAdmin, "BranchAdmin");

                    //ToDO
                    var verificationUrl = await SendVerificationEmail(newStoreAdmin, model.WebsiteLocation);

                    var msgBody = EmailHelper.ConfirmEmailTemplate(model.Email, verificationUrl);
                    _emailService.SendEmail(model.Email, model.LastName, "Account Confirmation", msgBody);


                    await AssignPagesToStoreAdminUser(newStoreAdmin.Id);
                }
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while saving BranchAdmin");
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
        public int DeleteBranchAdmin(DeleteBranchAdminDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.DELETE);
                param.Add("BranchAdminId", model.BranchAdminId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_BranchAdmin, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting BranchAdmin");
                return 0;
            }
        }
        public int Enable(DeleteBranchAdminDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.Enable);
                param.Add("BranchAdminId", model.BranchAdminId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_BranchAdmin, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting BranchAdmin");
                return 0;
            }
        }
        public int Disable(DeleteBranchAdminDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.Disable);
                param.Add("BranchAdminId", model.BranchAdminId);
                param.Add("DeletedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_BranchAdmin, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting BranchAdmin");
                return 0;
            }
        }

        public List<BranchAdminDTO> GetAllBranchAdmin(int branchId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETALL);
                param.Add("BranchID", branchId);
                var response = _dapper.GetAll<BranchAdminDTO>(ApplicationConstant.Sp_BranchAdmin, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a BranchAdmin");
                return null;
            }
        }

        public BranchAdminDTO GetBranchAdminById(int BranchAdminId)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYID);
                param.Add("BranchAdminId", BranchAdminId);
                var response = _dapper.Get<BranchAdminDTO>(ApplicationConstant.Sp_BranchAdmin, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a BranchAdmin");
                return new BranchAdminDTO();
            }
        }

        public int UpdateBranchAdmin(UpdateBranchAdminDTO model)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.UPDATE);
                param.Add("BranchAdminId", model.BranchAdminId);
                param.Add("FirstName", model.FirstName);
                param.Add("LastName", model.LastName);
                param.Add("Phone", model.Phone);
                param.Add("UpdatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_BranchAdmin, param, CommandType.StoredProcedure);
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating a BranchAdmin");
                return 0;
            }
        }

        public BranchAdminDTO getBranchAdminByPhone(string Phone)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETBYPHONE);
                param.Add("Phone", Phone);
                var response = _dapper.Get<BranchAdminDTO>(ApplicationConstant.Sp_BranchAdmin, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while retrieving a BranchAdmin");
                return new BranchAdminDTO();
            }
        }


        public async Task AssignPagesToStoreAdminUser(string userId)
        {
            var menuIds = ApplicationConstant.BranchAdminPages();
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
