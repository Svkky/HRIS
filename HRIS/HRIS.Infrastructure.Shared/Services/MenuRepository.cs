using Dapper;
using HRIS.Application.AppContants;
using HRIS.Application.DapperServices;
using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Menu;
using HRIS.Application.Enums;
using HRIS.Application.Interfaces;
using HRIS.Domain.Common;
using HRIS.Domain.Entities;
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
    public class MenuRepository : IMenuRepository
    {
        private readonly ILogger<MenuRepository> _logger;
        private readonly IAuthenticatedUserService _authenticatedUser;
        private string constring;
        IOptions<ConnectionStrings> myconnectionString;
        private readonly IDapper _dapper;
        private readonly IGenericRepositoryAsync<MenuSetup> _menuSetUpRepository;
        private readonly ApplicationDbContext _context;

        public MenuRepository(ILogger<MenuRepository> logger,
                             IAuthenticatedUserService authenticatedUser,
                             IOptions<ConnectionStrings> connectionString,
                             IDapper dapper,
                             IGenericRepositoryAsync<MenuSetup> menuSetUpRepository,
                             ApplicationDbContext context)
        {
            _logger = logger;
            _authenticatedUser = authenticatedUser;
            myconnectionString = connectionString;
            _dapper = dapper;
            _menuSetUpRepository = menuSetUpRepository;
            _context = context;
            constring = myconnectionString.Value.DefaultConnection;
        }

        //public List<MenuRole> GetMenu(string RoleID)
        //{
        //    try
        //    {
        //        int startCount = 0;
        //        var param = new DynamicParameters();
        //        param.Add("Status", Status.GETMENUBYROLEID);
        //        param.Add("RoleID", RoleID);
        //        var response = _dapper.GetAll<MenuRole>(ApplicationConstant.Sp_MenuSetup, param, commandType: CommandType.StoredProcedure);
        //        response.ForEach(c =>
        //        {
        //            c.Id = startCount++;
        //        });
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occured while getting a Menu");
        //        return new List<MenuRole>();
        //    }
        //}
        public async Task<List<MenuRole>> GetMenu(string RoleID)
        {
            try
            {
                int startCount = 0;
                var response = await _context.MenuSetup.Where(c => (c.ParentMenuId == null || c.ParentMenuId != "*") && c.RoleId == RoleID).Select(c => new MenuRole
                {
                    RoleID = c.RoleId,
                    ParentMenuId = c.ParentMenuId,
                    MenuId = c.MenuId,
                    MenuName = c.MenuName,
                    MenuUrl = c.MenuUrl,
                    IsActive = c.IsActive

                }).ToListAsync();
                response.ForEach(c =>
                {
                    c.Id = startCount++;
                });
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting a Menu");
                return new List<MenuRole>();
            }
        }

        public List<PermissionDTO> GetPermissionbyUserID(string UserID)
        {
            try
            {

                var param = new DynamicParameters();
                param.Add("Status", Status.GETMENUBYUSERID);
                param.Add("UserID", UserID);
                var response = _dapper.GetAll<PermissionDTO>(ApplicationConstant.Sp_MenuSetup, param, commandType: CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while loading permission");
                return new List<PermissionDTO>();
            }
        }

        public int CreateMenuSetup(CreateMenuDTO createMenuDTO)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("Status", Status.INSERT);
                param.Add("MenuId", createMenuDTO.MenuId);
                param.Add("MenuUrl", createMenuDTO.MenuUrl);
                param.Add("RoleName", createMenuDTO.RoleId);
                param.Add("MenuName", createMenuDTO.MenuName);
                param.Add("IconClass", createMenuDTO.IconClass);
                param.Add("ParentMenuId", createMenuDTO.ParentMenuId);
                param.Add("IconClass", createMenuDTO.IconClass);
                param.Add("CreatedBy", _authenticatedUser.UserId);

                var response = _dapper.Execute(ApplicationConstant.Sp_MenuSetup, param, CommandType.StoredProcedure);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating menu");
                return 0;
            }
        }

        public async Task<List<MenuRole>> GetPagesAssignedToUser(string UserID)
        {
            try
            {
                var response = new List<MenuRole>();
                var activePagesAssignedToUser = await _context.UsersRolePermission.Where(c => c.UserId == UserID && c.IsActive == true && c.IsDeleted == false).ToListAsync();
                foreach (var item in activePagesAssignedToUser)
                {
                    var menuDetail = await _context.MenuSetup.FirstOrDefaultAsync(c => c.MenuIdentity == item.MenuIdentity);
                    if (menuDetail.ParentMenuId != null)
                    {
                        var parentMenu = await _context.MenuSetup.FirstOrDefaultAsync(c => c.MenuId == menuDetail.ParentMenuId);
                        if (!response.Any(c => c.MenuId == parentMenu.MenuId))
                        {
                            //include the parent
                            response.Add(new MenuRole
                            {
                                Id = parentMenu.MenuIdentity,
                                MenuName = parentMenu.MenuName,
                                MenuId = parentMenu.MenuId,
                                MenuUrl = parentMenu.MenuUrl,
                                ParentMenuId = parentMenu.ParentMenuId,
                                UserID = UserID
                            });
                        }
                        response.Add(new MenuRole
                        {
                            Id = menuDetail.MenuIdentity,
                            MenuName = menuDetail.MenuName,
                            MenuId = menuDetail.MenuId,
                            MenuUrl = menuDetail.MenuUrl,
                            ParentMenuId = menuDetail.ParentMenuId,
                            UserID = UserID
                        });
                    }
                    else
                    {
                        response.Add(new MenuRole
                        {
                            Id = menuDetail.MenuIdentity,
                            MenuName = menuDetail.MenuName,
                            MenuId = menuDetail.MenuId,
                            MenuUrl = menuDetail.MenuUrl,
                            ParentMenuId = menuDetail.ParentMenuId,
                            UserID = UserID
                        });
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while creating menu");
                return null;
            }
        }

        public async Task<string> GetStoreName()
        {
            try
            {
                var store = await _context.StoreSetup.FirstOrDefaultAsync();
                if (store is null)
                    return "";
                return store.StoreName;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
