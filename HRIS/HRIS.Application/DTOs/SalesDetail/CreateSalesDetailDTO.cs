namespace HRIS.Application.DTOs.SalesDetail
{
    public class CreateSalesDetailDTO
    {
        public string productId { get; set; }
        public double quantity { get; set; }
        public double sellingPrice { get; set; }
        public double discount { get; set; }
        public double vatPercent { get; set; }
        public string subTotal { get; set; }
        public string vatValue { get; set; }
        public int branchId { get; set; }

    }
}
