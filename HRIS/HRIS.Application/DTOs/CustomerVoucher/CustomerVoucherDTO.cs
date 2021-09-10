using System;

namespace HRIS.Application.DTOs.CustomerVoucher
{
    public class CustomerVoucherDTO
    {
        public int CustomerVoucherId { get; set; }
        public string Customer { get; set; }
        public string Voucher { get; set; }
        public string FullName { get; set; }
        public string VoucherNo { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
