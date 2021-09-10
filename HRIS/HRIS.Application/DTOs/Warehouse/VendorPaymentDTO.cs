using System;

namespace HRIS.Application.DTOs.Warehouse
{
    public class VendorPaymentDTO
    {
        public int VendorPaymentMasterId { get; set; }
        public string BillNo { get; set; }
        public int? VendorId { get; set; }
        public int Cartons { get; set; }
        public int TotalItemsPerCarton { get; set; }
        public int TotalQuantity { get; set; }
        public string TotalAmount { get; set; }
        public string TotalPaid { get; set; }
        public string Balance { get; set; }
        public string InvoiceDocument { get; set; }
        public string VendorName { get; set; }
        public string ProductName { get; set; }
        public int StoreProductId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }


    }
}
