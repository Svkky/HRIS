using HRIS.Application.DTOs.Vat;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IVatService
    {
        int CreateVat(CreateVatDTO model);
        int UpdateVat(UpdateVatDTO model);
        int DeleteVat(DeleteVatDTO model);
        List<VatDTO> GetAllVat(int branchId);
        VatDTO GetVatById(int VatId, int branchId);
    }
}
