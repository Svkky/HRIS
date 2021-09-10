using System;

namespace HRIS.Application.DTOs.Audit
{
    public class CreateAuditDTO
    {
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }
}
