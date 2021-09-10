using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Domain.Entities
{
    public class ReceiptNoGenerator:BaseEntity
    {
        public int Id { get; set; }
        public string ReceiptNo { get; set; }
    }
}
