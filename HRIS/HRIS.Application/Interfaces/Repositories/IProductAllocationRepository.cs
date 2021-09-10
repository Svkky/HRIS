using HRIS.Application.DTOs.ProductAllocation;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IProductAllocationRepository
    {
        List<ProductAllocationDTO> GetProductAllocations();
        int CreateProductAllocation(CreateProductAllocationDTO request);
    }
}
