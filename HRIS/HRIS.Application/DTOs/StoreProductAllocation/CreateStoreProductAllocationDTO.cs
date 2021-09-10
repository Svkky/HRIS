using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.StoreProductAllocation
{
    public class CreateStoreProductAllocationDTO
    {
        public int ProductId { get; set; }
        public int BranchId { get; set; }
        public int AllocationQuantity { get; set; }
        public string Createdby { get; set; }
    }
}
