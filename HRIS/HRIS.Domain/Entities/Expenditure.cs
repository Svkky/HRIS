using System;

namespace HRIS.Domain.Entities
{
    public class Expenditure : BaseEntity
    {
        public int ExpenditureId { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
        public double Amount { get; set; }
        public DateTime ExpenditureDate { get; set; }
    }
}
