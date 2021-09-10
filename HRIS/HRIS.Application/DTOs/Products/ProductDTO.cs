using System;

namespace HRIS.Application.DTOs.Products
{
    public class ProductDTO
    {
        public int? productId { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public string name { get; set; }
        public float costPrice { get; set; }
        public float discount { get; set; }
        public int BranchId { get; set; }
        public int quantity { get; set; }
        public float sellingPrice { get; set; }
        public bool canExpire { get; set; }
        public DateTime manufactureDate { get; set; }
        public DateTime expiryDate { get; set; }
        public string variationQuantity { get; set; }
        public string location { get; set; }
        public int? vendorId { get; set; }
        public int criticalLevel { get; set; }
        public int categoryId { get; set; }
        public int subCategoryId { get; set; }
        public double vat { get; set; }
        public int? productTypeVariationId { get; set; }
        public double vatPercent { get; set; }
        public double totalAmount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ProductWithVariation { get; set; }


    }
    public class BranchProductDTO
    {
        public int BranchProductId { get; set; }
        public string StoreProductId { get; set; }
        public string SellingPrice { get; set; }
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
        public string QuantityRemaning { get; set; }
        public int BranchId { get; set; }
        public bool IsConfigured { get; set; }
        public string CreatedOn { get; set; }


        //public Category Category { get; set; }
        //public SubCategory SubCategory { get; set; }


    }
    public class EditBranchProductDTO
    {
        public string BranchProductId { get; set; }
        public double SellingPrice { get; set; }
        public string Discount { get; set; }
        public bool CanExpire { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? ProductTypeVariationId { get; set; }
        public string VariationQuantity { get; set; }
        public string CriticalLevel { get; set; }
        public string VatPercent { get; set; }
        public string BranchId { get; set; }
    }
}
