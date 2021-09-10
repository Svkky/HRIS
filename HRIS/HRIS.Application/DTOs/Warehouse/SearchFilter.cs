using System;

namespace HRIS.Application.DTOs.Warehouse
{
    public class SearchFilter
    {
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public string SupplierId { get; set; }
        public string ProductName { get; set; }
        public string TransactionType { get; set; }
    }
}
