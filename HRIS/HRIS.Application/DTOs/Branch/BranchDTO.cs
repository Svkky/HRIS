using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Branch
{
   public class BranchDTO
    {
        public int branchID { get; set; }
        public string branchName { get; set; }
        public string phoneNumber { get; set; }
        public string location { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime deletedOn { get; set; }
        public DateTime expenditureDate { get; set; }
        public DateTime updatedOn { get; set; }
        public string updatedBy { get; set; }
        public string createdBy { get; set; }
        public bool isDeleted { get; set; }
        public string deletedBy { get; set; }
    }
}
