using HRIS.Application.DTOs.Account;
using HRIS.Application.DTOs.Branch;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IBranchRepository
    {
        int CreateBranch(BranchDTO model);
        int DeleteBranch(int BranchID);
        List<BranchDTO> GetAllBranch();
        BranchDTO GetAllBranchByID(int BranchID);
        List<BranchAdminDT> GetAllBranchAdmin(int BranchID);
        int UpdateBranch(UpdateBranchDTO model);
        int AssignUserToBranch(AssignUserToBranchDTO model);
        int RemoveUserFromBranch(string UserID);
    }
}