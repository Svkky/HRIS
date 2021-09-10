using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.StoreProductAllocation
{
    public class StoreProductAllocationDTO
    {
        public int? ProductAllocationId { get; set; }
        public int ProductId { get; set; }
		public string ProductName { get; set; }
        public int BranchId { get; set; }
		public string BranchName { get; set; }
        public int AllocationQuantity { get; set; }
        public string Createdby { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Updatedby { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
