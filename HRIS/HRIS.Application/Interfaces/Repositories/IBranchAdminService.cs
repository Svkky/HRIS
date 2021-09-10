using HRIS.Application.DTOs.BranchAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IBranchAdminService
    {
        Task<int> CreateBranchAdmin(CreateBranchAdminDTO model);
        int UpdateBranchAdmin(UpdateBranchAdminDTO model);
        int DeleteBranchAdmin(DeleteBranchAdminDTO model);
        int Disable(DeleteBranchAdminDTO model);
        int Enable(DeleteBranchAdminDTO model);
        List<BranchAdminDTO> GetAllBranchAdmin(int branchId);
        BranchAdminDTO GetBranchAdminById(int BranchAdminId);
        BranchAdminDTO getBranchAdminByPhone(string Phone);
    }
}
