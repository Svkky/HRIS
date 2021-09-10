using HRIS.Application.DTOs.StoreProductAllocation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IStoreProductAllocationRepository
    {
        int CreateStoreAllocation(CreateStoreProductAllocationDTO model);
        List<StoreProductAllocationDTO> GetAllStoreAllocation();
    }
}

