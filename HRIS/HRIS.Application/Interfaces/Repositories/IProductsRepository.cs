using HRIS.Application.DTOs.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Infrastructure.Interfaces.Repositories
{
    public interface IProductsRepository
    {

        Task<BranchProductDTO> GetProductByID(int ProductID, int branchId);

        Task<List<BranchProductDTO>> GetAllProducts(int branchId);
        Task<int> UpdateProductAsync(EditBranchProductDTO model);
        Task<List<ProductAllocationDTO>> GetBranchProducts(int branchId);
    }
}