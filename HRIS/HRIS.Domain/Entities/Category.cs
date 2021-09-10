using System;
using System.Collections.Generic;

namespace HRIS.Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        //public int BranchId { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        //public List<BranchProduct> Products { get; set; }
    }
}
