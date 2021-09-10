using System;

namespace HRIS.Domain.Entities
{
    public class Audit
    {
        public int AuditId { get; set; }
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }
}
