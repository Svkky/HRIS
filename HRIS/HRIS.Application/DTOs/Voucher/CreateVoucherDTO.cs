using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Voucher
{
    public class CreateVoucherDTO
    {
       
        public string description { get; set; }
        public double amount { get; set; }
        public DateTime expiryDate { get; set; }
        public int branchId { get; set; }
    }
}
