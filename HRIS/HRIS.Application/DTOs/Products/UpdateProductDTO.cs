using System;

namespace HRIS.Application.DTOs.Products
{
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public float CostPrice { get; set; }
        public float SellingPrice { get; set; }
        public int CriticalLevel { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class UpdateBranchProductDTO
    {
        public int BranchProductId { get; set; }
        public string UpdatedBy { get; set; }
        public string Name { get; set; }
        public float SellingPrice { get; set; }
        public int CriticalLevel { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
    public class ProductAllocationDTO
    {
        public int ProductAllocationUseId { get; set; }
        public string ProductName { get; set; }
        public double QuantityRemaining { get; set; }
    }
}
