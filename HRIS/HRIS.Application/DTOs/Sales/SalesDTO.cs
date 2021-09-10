using System;
using System.Collections.Generic;

namespace HRIS.Application.DTOs.Sales
{
    public class SalesDTO
    {
        public string Name { get; set; }
        public string Fullname { get; set; }
        public string BranchName { get; set; }
        public string StoreName { get; set; }
        public int Quantity { get; set; }
        public string BillNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public double SubTotal { get; set; }
        public double TotalDiscount { get; set; }
        public double TotalPaid { get; set; }
        public double TotalAmount { get; set; }
        public int Change { get; set; }
        public double TotalVat { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DatePaid { get; set; }
    }

    public class SalesVm
    {
        public string BillNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalPaid { get; set; }
        public string TotalAmount { get; set; }
        public string Change { get; set; }
        public string TotalVat { get; set; }
        public string Fullname { get; set; }
        public string BranchName { get; set; }
        public string StoreName { get; set; }
        public string PhoneNumber { get; set; }
        public string DatePaid { get; set; }
        public string ModeOfPayment { get; set; }
        public List<SalesDetailVm> SalesDetailVm { get; set; }

    }
    public class BranchSales
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int TotalOrders { get; set; }
        public string TotalAmount { get; set; }

    }
    public class SalesDetailVm
    {
        public string Name { get; set; }

        public double Quantity { get; set; }
        public double SubTotal { get; set; }

    }
    public class SalesDetailVM
    {
        public int SaleDetailId { get; set; }
        public string Name { get; set; }

        public double Quantity { get; set; }
        public string SubTotal { get; set; }
        public string SellingPrice { get; set; }
        public string Discount { get; set; }
        public string VatPercent { get; set; }
        public string VatValue { get; set; }

    }
}
