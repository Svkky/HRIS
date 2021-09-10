namespace HRIS.Domain.Entities
{
    public class StoreSetup : BaseEntity
    {
        public int StoreSetupId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreEmail { get; set; }
        // public bool EmailSent { get; set; }
    }
}
