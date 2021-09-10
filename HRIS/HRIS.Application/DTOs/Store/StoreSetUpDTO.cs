using System;

namespace HRIS.Application.DTOs
{
    public class StoreSetUpDTO
    {
        public int storeSetupId { get; set; }
        public string storeName { get; set; }
        public string WebsiteLocation { get; set; }
        public string storeAddress { get; set; }
        public string storePhone { get; set; }
        public string storeEmail { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
