using HRIS.Application.DTOs.Voucher;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface IVoucherService
    {
        int CreateVoucher(CreateVoucherDTO model);
        int UpdateVoucher(UpdateVoucherDTO model);
        int DeleteVoucher(DeleteVoucherDTO model);
        List<VoucherDTO> GetAllVoucher(int branchId);
        VoucherDTO GetVoucherById(int VoucherId,int branchId);
        Task<VoucherDTO> GetVoucherByVoucherNo(string voucherNo, string GetVoucherByVoucherNo);
    }
}
