using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Vat
{
    public class UpdateVatDTO
    {
        public int vatId { get; set; }
        public string Name { get; set; }
        public double percentage { get; set; }
    }
}
