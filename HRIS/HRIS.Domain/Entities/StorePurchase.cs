namespace HRIS.Domain.Entities
{
    public class StorePurchase : BaseEntity
    {
        public int StorePurchaseId { get; set; }
        public string PurchaseNo { get; set; }
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }
        public int Quantity { get; set; }
        public double TotalAmount { get; set; }
    }
}
