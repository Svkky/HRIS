using System;

namespace HRIS.Domain.Entities
{
    public class VendorReturn : BaseEntity
    {
        public int VendorReturnId { get; set; }
        public string PurchaseNo { get; set; }
        public double Quantity { get; set; }
        public DateTime DateReturned { get; set; }
    }
}
