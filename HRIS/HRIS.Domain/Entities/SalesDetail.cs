using System;

namespace HRIS.Domain.Entities
{
    public class SalesDetail : BaseEntity
    {
        public int SalesDetailId { get; set; }
        public string BillNumber { get; set; }
        public int StoreProductId { get; set; }
        public double Quantity { get; set; }
        public double SellingPrice { get; set; }
        public double Discount { get; set; }
        public double VatPercent { get; set; }
        public double VatValue { get; set; }
        public double SubTotal { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public bool? IsVatPaid { get; set; }

        public StoreProduct StoreProduct { get; set; }
    }
}
