namespace HRIS.Application.DTOs.StoreProduct
{
    public class CreateStoreProductDTO
    {
        public int CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string ProductName { get; set; }
        public string Createdby { get; set; }
    }
}
