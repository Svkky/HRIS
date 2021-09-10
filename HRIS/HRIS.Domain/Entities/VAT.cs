namespace HRIS.Domain.Entities
{
    public class VAT : BaseEntity
    {
        public int VATId { get; set; }
        public string Name { get; set; }
        public double percentage { get; set; }
        public bool isDisable { get; set; }
    }
}
