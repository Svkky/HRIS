namespace HRIS.Application.DTOs.Supplier
{
    public class CreateSupplierDTO
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int BranchId { get; set; }
    }
}
