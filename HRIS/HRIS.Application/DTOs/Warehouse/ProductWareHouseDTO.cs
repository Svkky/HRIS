using System;

namespace HRIS.Application.DTOs.Warehouse
{
    public class ProductWareHouseDTO
    {
        public int ProductWareHouseId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string BalanceBefore { get; set; }
        public string AllocatedQuantity { get; set; }
        public string TransactionType { get; set; }
        public string BalanceAfter { get; set; }
        public string CreatedOn { get; set; }
        public DateTime DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string BranchName { get; set; }

    }
}
