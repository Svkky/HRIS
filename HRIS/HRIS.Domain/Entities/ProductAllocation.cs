namespace HRIS.Domain.Entities
{
    public class ProductAllocation : BaseEntity
    {
        public int ProductAllocationId { get; set; }
        public int ProductId { get; set; }
        public int AllocationQuantity { get; set; }
    }
}
