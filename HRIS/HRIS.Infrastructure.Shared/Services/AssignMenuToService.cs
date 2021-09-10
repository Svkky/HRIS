using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Menu;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Application.Interfaces.Repositories;
using HRIS.Domain.Common;
using HRIS.Domain.Entities;
using HRIS.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Shared.Services
{
    public class AssignMenuToService : IAssignMenuToUserService
    {
        private readonly IDapper dapper;
        private readonly ApplicationDbContext _context;
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AssignMenuToService> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        //  private readonly IOptions<ConnectionStrings> _connectionString;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;

        public AssignMenuToService(ILogger<AssignMenuToService> logger,
                               IAuthenticatedUserService authenticatedUser,
                               IOptions<ConnectionStrings> connectionString,
                               IDapper dapper,
                               ApplicationDbContext context,
                               IAuditRepository auditRepository)
        {
            this.dapper = dapper;
            _context = context;
            _auditRepository = auditRepository;
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            constring = myconnectionString.Value.DefaultConnection;

        }
        //public int CreateMenuRequest(MenuRequest model)
        //{
        //    var response = 0;
        //    try
        //    {
        //        foreach (var item in model.menus)
        //        {
        //            var param = new DynamicParameters();
        //            param.Add("Status", Status.INSERT);
        //            param.Add("UserId", model.userId);
        //            param.Add("RoleID", model.roleId);
        //            param.Add("menuId", item);
        //            param.Add("CreatedBy", _authenticatedUser.UserId);
        //            response = dapper.Execute(ApplicationConstant.SP_AssignPermissionToUser, param, CommandType.StoredProcedure);

        //        }

        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occured while Assigning menu to users");
        //        return 0;
        //    }

        //}
        public async Task<int> CreateMenuRequest(MenuRequest model)
        {
            try
            {
                var counter = 0;
                foreach (var menu in model.menus)
                {
                    var menuIdentity = await _context.MenuSetup.FirstOrDefaultAsync(c => c.MenuId == menu);
                    var IsMenuAssignedPreviously = await _context.UsersRolePermission.AnyAsync(c => c.MenuIdentity == menuIdentity.MenuIdentity && c.UserId == model.userId && c.IsDeleted == false);
                    if (!IsMenuAssignedPreviously)
                    {
                        var userRolePermission = new UsersRolePermission
                        {
                            MenuIdentity = menuIdentity.MenuIdentity,
                            UserId = model.userId,
                            IsActive = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = model.userId
                        };
                        await _context.UsersRolePermission.AddAsync(userRolePermission);
                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            string action = $"Assigned {menuIdentity.MenuName} to {model.userId}";
                            await _auditRepository.CreateAudit(model.userId, action);
                        }
                        counter++;
                    }
                    else
                    {
                        counter = -1;
                    }
                }

                return counter;

            }
            catch (Exception ex)
            {

                return 0; ;
            }
        }

        public int DeleteMenuRequest(MenuRequest model)
        {
            var response = 0;
            try
            {
                foreach (var item in model.menus)
                {
                    var param = new DynamicParameters();
                    param.Add("Status", Status.DELETE);
                    param.Add("userId", model.userId);
                    param.Add("roleId", model.roleId);
                    param.Add("menuId", item);
                    param.Add("DeletedBy", _authenticatedUser.UserId);
                    response = dapper.Execute(ApplicationConstant.SP_AssignPermissionToUser, param, CommandType.StoredProcedure);
                }

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while deleting menu assign to users");
                return 0;
            }
        }

        public int UpdateRequest(MenuRequest model)
        {
            var response = 0;
            try
            {
                model.menus.ForEach(x =>
                {
                    var param = new DynamicParameters();
                    param.Add("Status", Status.INSERT);
                    param.Add("UserId", model.userId);
                    param.Add("menuId", x);
                    param.Add("UpdatedBy", _authenticatedUser.UserId);
                    response = dapper.Execute(ApplicationConstant.SP_AssignPermissionToUser, param, CommandType.StoredProcedure);

                });

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while updating Assigning menu to users");
                return 0;
            }
        }
    }
}
