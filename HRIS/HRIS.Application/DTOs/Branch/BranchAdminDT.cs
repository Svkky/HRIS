using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Branch
{
   public class BranchAdminDT
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int branchID { get; set; }
        public string Phone { get; set; }
        public string UserId { get; set; }      
        public string Email { get; set; }
    }
}
