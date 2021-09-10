using HRIS.Application.DTOs.CustomerVoucher;
using System.Collections.Generic;

namespace HRIS.Application.Interfaces.Repositories
{
    public interface ICustomerVoucherService
    {
        int CreateCustomerVoucher(CreateCustomerVoucherDTO model);
        int DeleteCustomerVoucher(DeleteCustomerVoucherDTO model);
        List<CustomerVoucherDTO> GetAllCustomerVoucher(int branchId);
        CustomerVoucherDTO GetCustomerVoucherById(int customerVoucherId,int branchId);
    }
}
