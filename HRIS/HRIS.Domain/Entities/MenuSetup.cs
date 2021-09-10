using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Domain.Entities
{
    public class MenuSetup : BaseEntity
    {
        [Key]
        public int MenuIdentity { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string ParentMenuId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string MenuUrl { get; set; }
        public string IconClass { get; set; }
        public bool IsActive { get; set; }

        public new int? BranchId { get; set; }
    }
}
