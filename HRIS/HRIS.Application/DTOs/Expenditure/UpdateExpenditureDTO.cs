using System;

namespace HRIS.Application.DTOs.Expenditure
{
    public class UpdateExpenditureDTO
    {
        public int ExpenditureID { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }

    }
    public class DeleteExpenditureDTO
    {
        public int ExpenditureID { get; set; }


    }
}
