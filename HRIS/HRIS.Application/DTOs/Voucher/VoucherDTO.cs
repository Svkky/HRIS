using System;

namespace HRIS.Application.DTOs.Voucher
{
    public class VoucherDTO
    {
        public int voucherId { get; set; }
        public string description { get; set; }
        public string voucherNo { get; set; }
        public double amount { get; set; }
        public double balance { get; set; }
        public bool isActive { get; set; }
        public DateTime expiryDate { get; set; }
        public string amt { get; set; }
        public string expiry { get; set; }

    }
}
