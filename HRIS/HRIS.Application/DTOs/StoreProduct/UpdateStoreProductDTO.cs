namespace HRIS.Application.DTOs.StoreProduct
{
    public class UpdateStoreProductDTO
    {
        public int StoreProductId { get; set; }
        public int CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string ProductName { get; set; }
        public string Updatedby { get; set; }
    }
}
