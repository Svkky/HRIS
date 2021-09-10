using System;

namespace HRIS.Application.DTOs.ProductAllocation
{
    public class ProductAllocationDTO
    {
        public int ProductAllocationId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string DateCreated { get; set; }
        public double AllocationQuantity { get; set; }
    }
}
