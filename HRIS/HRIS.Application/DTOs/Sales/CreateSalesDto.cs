using HRIS.Application.DTOs.SalesDetail;
using System;
using System.Collections.Generic;

namespace HRIS.Application.DTOs.Sales
{
    public class CreateSalesDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string ModeOfPayment { get; set; }
        public bool isVoucherUsed { get; set; }
        public int? CustomerVoucherId { get; set; }
        public bool isPaid { get; set; }
        public string TotalVat { get; set; }
        public string TotalDiscount { get; set; }
        public DateTime DatePaid { get; set; }
        public string TotalPaid { get; set; }
        public int branchId { get; set; }
    }

    public class ParentSales
    {
        public CreateSalesDto sales { get; set; }
        public List<CreateSalesDetailDTO> detail { get; set; }
    }
}
