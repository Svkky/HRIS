using System;

namespace HRIS.Domain.Entities
{
    public class BranchProduct : BaseEntity
    {
        public int BranchProductId { get; set; }
        public double SellingPrice { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
        public bool CanExpire { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int? ProductTypeVariationId { get; set; }
        public string VariationQuantity { get; set; }
        public int CriticalLevel { get; set; }
        public double VatPercent { get; set; }
        public int StoreProductId { get; set; }
        public ProductTypeVariation ProductVariation { get; set; }
        public StoreProduct StoreProduct { get; set; }
        public bool IsConfigured { get; set; }

    }
}
