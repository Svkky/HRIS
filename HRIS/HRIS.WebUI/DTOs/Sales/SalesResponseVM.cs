using System;
using System.Collections.Generic;

namespace HRIS.WebUI.DTOs.Sales
{
    public class SalesResponseVM
    {
        public string BillNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string TotalDiscount { get; set; }
        public string TotalPaid { get; set; }
        public string TotalVat { get; set; }
        public string Fullname { get; set; }
        public string PhoneNumber { get; set; }
        public string TotalAmount { get; set; }
        public string Change { get; set; }
        public string BranchName { get; set; }
        public string StoreName { get; set; }
        public DateTime DatePaid { get; set; }
        public List<SalesDetailVM> SalesDetailVm { get; set; }
    }

}
