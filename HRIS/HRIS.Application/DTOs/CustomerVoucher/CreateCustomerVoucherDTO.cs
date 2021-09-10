namespace HRIS.Application.DTOs.CustomerVoucher
{
    public class CreateCustomerVoucherDTO
    {
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public int BranchId { get; set; }
    }
}
