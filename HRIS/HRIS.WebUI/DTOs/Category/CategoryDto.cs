using System.Collections.Generic;

namespace HRIS.WebUI.DTOs.Category
{

    public class CategoryDto
    {
        public int categoryId { get; set; }
        public string description { get; set; }
    }
    public class SubCategoryDto
    {
        public int subCategoryId { get; set; }
        public string subCategoryName { get; set; }
    }

    public class Base<T>
    {
        public List<T> data { get; set; }
        public int totalCount { get; set; }
        public int groupCount { get; set; }
        public object summary { get; set; }
    }
}
