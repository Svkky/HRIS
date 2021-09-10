using System.ComponentModel.DataAnnotations;

namespace HRIS.Application.DTOs.Account
{
    public class RegisterRequest
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string branchId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string UserName { get; set; }
        [Required]
        public string webUrl { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
