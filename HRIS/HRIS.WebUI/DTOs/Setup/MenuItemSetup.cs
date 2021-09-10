namespace HRIS.WebUI.DTOs.Setup
{
    public class MenuItemSetup
    {
        public int Id { get; set; }
        public string RoleID { get; set; }
        public string UserID { get; set; }
        public string MenuId { get; set; }
        public string MenuName { get; set; }
        public string ParentMenuId { get; set; }
        public string MenuFileName { get; set; }
        public string MenuUrl { get; set; }
        public string ImgClass { get; set; }
        public string InstitutionCode { get; set; }
        public bool IsActive { get; set; }

    }
}
