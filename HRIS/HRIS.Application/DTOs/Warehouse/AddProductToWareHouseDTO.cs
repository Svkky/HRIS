using System;

namespace HRIS.Application.DTOs.Warehouse
{
    public class AddProductToWareHouseDTO
    {
        public int? VendorId { get; set; }
        public int Cartons { get; set; }
        public int TotalItemsPerCarton { get; set; }
        public int TotalQuantity { get; set; }
        public double TotalAmount { get; set; }
        public double TotalPaid { get; set; }
        public double Balance { get; set; }
        public string InvoiceDocument { get; set; }
        public int StoreProductId { get; set; }
        public string BillNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
    }
}
