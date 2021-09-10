namespace HRIS.Application.DTOs.SubCategory
{
    public class SubCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
    }

    public class SubCategoryVm
    {
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string Description { get; set; }
    }
}
