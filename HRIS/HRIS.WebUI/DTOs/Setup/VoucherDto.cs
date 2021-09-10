using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebUI.DTOs.Setup
{
    public class VoucherDto
    {
        public int voucherId { get; set; }
        public string description { get; set; }
        public string voucherNo { get; set; }
        public double amount { get; set; }
        public DateTime expiryDate { get; set; }
    }
}
