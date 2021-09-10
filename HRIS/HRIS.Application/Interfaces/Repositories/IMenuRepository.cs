using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Menu;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Interfaces.Repositories
{
    public interface IMenuRepository
    {
        Task<List<MenuRole>> GetMenu(string RoleID);
        List<PermissionDTO> GetPermissionbyUserID(string UserID);
        Task<List<MenuRole>> GetPagesAssignedToUser(string UserID);
        Task<string> GetStoreName();
        int CreateMenuSetup(CreateMenuDTO createMenuDTO);
    }
}