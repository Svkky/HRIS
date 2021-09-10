using System.ComponentModel.DataAnnotations;

namespace HRIS.Application.DTOs.Menu
{
    public class CreateMenuDTO
    {
        [Display(Name = "Menu ID")]
        public string MenuId { get; set; }
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }
        [Display(Name = "Parent Menu ID")]
        public string ParentMenuId { get; set; }
        [Display(Name = "Role Name")]
        public string RoleId { get; set; }
        [Display(Name = "Menu Page Url")]
        public string MenuUrl { get; set; }
        [Display(Name = "Menu Icon Class")]
        public string IconClass { get; set; }



    }
}
