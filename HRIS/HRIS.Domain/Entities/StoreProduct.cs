using System;
using System.Collections.Generic;

namespace HRIS.Domain.Entities
{
    public class StoreProduct
    {
        public int StoreProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public List<VendorPaymentMaster> VendorPaymentMaster { get; set; } = new List<VendorPaymentMaster>();
        public List<ProductWareHouse> ProductWareHouse { get; set; } = new List<ProductWareHouse>();
    }
}
