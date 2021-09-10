using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Sales
{
    public class UpdateSalesDto
    {
        public int salesID { get; set; }
        public int branchID { get; set; }
        public int CustomerID { get; set; }
        public string messages { get; set; }
        public bool isVoucherUsed { get; set; }
        public string CustomerVoucherId { get; set; }
        public bool isPaid { get; set; }
        public float TotalVat { get; set; }
        public float TotalDiscount { get; set; }
        public float vatPercent { get; set; }
        public DateTime DatePaid { get; set; }
        public float TotalPaid { get; set; }
    }
}
