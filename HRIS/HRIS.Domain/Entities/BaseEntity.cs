using System;

namespace HRIS.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOn { get; set; }
        public string DeletedBy { get; set; }
        public int BranchId { get; set; }
    }

    public class ReadConnectionString
    {
        public string ConnectionString { get; set; }
    }
}
