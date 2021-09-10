using System;

namespace HRIS.Domain.Entities
{
    public class Sales : BaseEntity
    {
        public int SalesId { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalPaid { get; set; }
        public double TotalAmount { get; set; }
        public double TotalVat { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsVoucherUsed { get; set; }
        public int? CustomerVoucherId { get; set; }
        public bool IsPaid { get; set; }
        public string RecieptNumber { get; set; }
        public string BillNumber { get; set; }
        public DateTime DatePaid { get; set; }
        public double TotalBalance { get; set; }
        public int Change { get; set; }
        public string ModeOfPayment { get; set; }
        public Customer Customer { get; set; }
        public Branch Branch { get; set; }
    }
}
