using System.Collections.Generic;

namespace HRIS.WebUI.DTOs.Authentication
{
    public class LoginResponse
    {
        public string id { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public int branchId { get; set; }
        public IList<string> roles { get; set; }
        public bool isVerified { get; set; }
        public string jwToken { get; set; }
    }
}
