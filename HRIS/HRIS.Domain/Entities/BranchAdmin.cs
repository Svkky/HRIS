using System;

namespace HRIS.Domain.Entities
{
    public class BranchAdmin : BaseEntity
    {
        public int BranchAdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public string DisabledBy { get; set; }
        public string EnabledBy { get; set; }
        public DateTime DateEnabled { get; set; }
        public DateTime DateDisbled { get; set; }
    }
}
