using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Vat
{
    public class CreateVatDTO
    {
        public string Name { get; set; }
        public double percentage { get; set; }
        public int branchId { get; set; }
    }
}
