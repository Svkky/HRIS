using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRIS.WebUI.DTOs.Setup
{
    public class ProductTypeVariationDto
    {
        public int productTypeVariationId { get; set; }
        public string description { get; set; }
        public DateTime createdOn { get; set; }
        public string createdBy { get; set; }
    }
}
