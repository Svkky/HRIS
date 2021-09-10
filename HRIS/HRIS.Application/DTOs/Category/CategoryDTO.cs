using System.Collections.Generic;

namespace HRIS.Application.DTOs.Category
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
    }

    public class CategoryVm
    {
        public int statusId { get; set; }
        public string statusMessage { get; set; }
        public IList<CategoryDTO> recordResponseObject { get; set; }
        public bool isSuccessful { get; set; }
    }
}
