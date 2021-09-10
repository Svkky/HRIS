using HRIS.Application.DTOs.Supplier;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface ISupplierService
    {
        int CreateSupplier(CreateSupplierDTO model);
        int UpdateSupplier(UpdateSupplierDTO model);
        int DeleteSupplier(DeleteSupplierDTO model);
        List<SupplierDTO> GetAllSupplier();
        SupplierDTO GetSupplierById(int supplierId);
    }
}
