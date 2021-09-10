namespace HRIS.Application.DTOs.ProductAllocation
{
    public class CreateProductAllocationDTO
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public double AllocationQuantity { get; set; }
    }
}
