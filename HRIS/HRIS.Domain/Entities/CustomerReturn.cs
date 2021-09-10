using System;

namespace HRIS.Domain.Entities
{
    public class CustomerReturn : BaseEntity
    {
        public int CustomerReturnId { get; set; }
        public string BillNo { get; set; }
        public double TotalQuantityReturned { get; set; }
        public DateTime DateReturned { get; set; }
        public bool HasBeenRefunded { get; set; }
        public DateTime DateRefunded { get; set; }
    }
}
