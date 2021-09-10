using HRIS.Application.DTOs.Menu;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IAssignMenuToUserService
    {
        Task<int> CreateMenuRequest(MenuRequest model);
        // int CreateMenuRequest(MenuRequest model);
        int UpdateRequest(MenuRequest model);
        int DeleteMenuRequest(MenuRequest model);
    }
}
