using System;

namespace HRIS.Domain.Entities
{
    public class ProductWareHouse
    {
        public int ProductWareHouseId { get; set; }
        public int StoreProductId { get; set; }
        public int BalanceBefore { get; set; }
        public int Quantity { get; set; }
        public string TransactionType { get; set; }
        public int BalanceAfter { get; set; }
        public int? BranchId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public StoreProduct StoreProduct { get; set; }
        public Branch Branch { get; set; }
    }
}
