namespace HRIS.Domain.Entities
{
    public class Branch : BaseEntity
    {
        public string BranchName { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public int StoreId { get; set; }
    }
}
