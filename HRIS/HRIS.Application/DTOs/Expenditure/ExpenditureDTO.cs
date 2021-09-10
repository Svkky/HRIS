using System;

namespace HRIS.Application.DTOs.Expenditure
{
    public class ExpenditureDTO
    {
        public int ExpenditureID { get; set; }
        public int BranchId { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime ExpenditureDate { get; set; }
        public string ExpenditureDatee { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }

    }
    public class CreateExpenditureDTO
    {
        public string Comment { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public string ExpenditureDate { get; set; }

    }
}
