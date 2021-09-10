namespace HRIS.Domain.Entities
{
    public class ProductAllocationUse : BaseEntity
    {
        public int ProductAllocationUseId { get; set; }
        public int StoreProductId { get; set; }
        public double AllocationQuantityRemaining { get; set; }
        public StoreProduct StoreProduct { get; set; }
    }
}
