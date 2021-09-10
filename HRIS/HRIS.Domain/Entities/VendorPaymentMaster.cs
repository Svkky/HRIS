using System;

namespace HRIS.Domain.Entities
{
    public class VendorPaymentMaster
    {
        public int VendorPaymentMasterId { get; set; }
        public string BillNo { get; set; }
        public int? SupplierId { get; set; }
        public int Cartons { get; set; }
        public int TotalItemsPerCarton { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double TotalPaid { get; set; }
        public double Balance { get; set; }
        public string InvoiceDocument { get; set; }
        public int StoreProductId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public StoreProduct StoreProduct { get; set; }
        public Supplier Supplier { get; set; }
    }
}
