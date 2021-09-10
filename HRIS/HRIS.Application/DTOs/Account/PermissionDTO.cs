using System;

namespace HRIS.Application.DTOs.Account
{
    public class PermissionDTO
    {
        public int UserPermissionID { get; set; }
        public int MenuIdentity { get; set; }
        public int UserID { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDeleted { get; set; }
        public DateTime DateUpdated { get; set; }
        public string DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string UpdatedBy { get; set; }
    }
}
