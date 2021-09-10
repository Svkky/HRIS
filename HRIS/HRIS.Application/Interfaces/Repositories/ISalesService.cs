using HRIS.Application.DTOs.Sales;
using HRIS.Application.DTOs.Warehouse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface ISalesService
    {
        Task<List<BranchSales>> BranchSales();
        SalesVm CreateSales(ParentSales model);
        Task<SalesVm> AddSaleAsync(ParentSales model);
        SalesVm GetAllSales(string BillNumber);
        Task<List<SalesDetailVM>> GetSalesDetails(string billNumber);
        Task<SalesVm> GetAllSaleAsync(string BillNumber);
        Task<List<SalesVm>> GetSales(string loggedInUserId, string brnchId);
        Task<List<SalesVm>> FilterSales(SearchFilter searchFilter, string loggedInUserId, string brnchId);

    }
}
