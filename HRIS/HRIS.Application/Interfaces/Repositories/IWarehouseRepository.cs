using HRIS.Application.DTOs.Warehouse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IWarehouseRepository
    {
        Task<List<VendorPaymentDTO>> GetVendorPayments();
        Task<List<ProductWareHouseDTO>> GetWareHouseProduct();
        Task<int> AddProductToWarehouse(AddProductToWareHouseDTO request);
        Task<int> GetTotalRemainingProduct(int ProductId);
        Task<List<VendorPaymentDTO>> FilterPayments(SearchFilter searchFilter);
        Task<List<ProductWareHouseDTO>> FilterProductsInWarehouse(SearchFilter searchFilter);
    }
}
