using System;

namespace HRIS.Application.DTOs.ProductTypeVariation
{
    public class ProductTypeVariationDTO
    {
        public int productTypeVariationId { get; set; }
        public string description { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
    }
}
