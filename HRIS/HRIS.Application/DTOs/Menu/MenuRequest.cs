using System.Collections.Generic;

namespace HRIS.Application.DTOs.Menu
{
    public class MenuRequest
    {
        public string roleId { get; set; }
        public string userId { get; set; }
        public List<string> menus { get; set; }
    }
}
