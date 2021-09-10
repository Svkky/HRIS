using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Branch
{
    public class UpdateBranchDTO
    {

        public int BranchID { get; set; }
        public string userID { get; set; }
        public string BranchName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}
