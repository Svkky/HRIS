using System;
using System.Collections.Generic;
using System.Text;

namespace HRIS.Application.DTOs.Store
{
   public class StoreUpdateDTO
    {
        public int StoreSetupId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StorePhone { get; set; }
        public string StoreEmail { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
