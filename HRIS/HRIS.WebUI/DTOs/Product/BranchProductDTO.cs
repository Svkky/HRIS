using System;

namespace HRIS.WebUI.DTOs.Product
{
    public class BranchProductDTO
    {
        public int BranchProductId { get; set; }
        public int ProductionAllocationUseId { get; set; }
        public double SellingPrice { get; set; }
        public double Discount { get; set; }
        public bool canExpire { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string ProductTypeVariation { get; set; }
        public string ProductTypeVariationId { get; set; }
        public string VariationQuantity { get; set; }
        public int CriticalLevel { get; set; }
        public double VatPercent { get; set; }
        public string ProductName { get; set; }
        public double QuantityRemaning { get; set; }
        public int BranchId { get; set; }
        //public Category Category { get; set; }
        //public SubCategory SubCategory { get; set; }


    }
}
