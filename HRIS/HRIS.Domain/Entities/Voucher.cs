using System;
using System.Collections.Generic;

namespace HRIS.Domain.Entities
{
    public class Voucher : BaseEntity
    {
        public int VoucherId { get; set; }
        public string Description { get; set; }
        public string VoucherNo { get; set; }
        public double Amount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public List<CustomerVoucher> CustomerVouchers { get; set; }
    }
}
