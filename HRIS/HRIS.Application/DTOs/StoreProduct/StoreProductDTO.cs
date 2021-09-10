using System;

namespace HRIS.Application.DTOs.StoreProduct
{
    public class StoreProductDTO
    {
        public int? StoreProductId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string ProductName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Createdby { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Updatedby { get; set; }
        public int IsDeleted { get; set; }

        public string SubCategoryNames
        {
            get { return SubCategoryName == null ? "N/A" : SubCategoryName; }
        }
    }
}
