using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HRIS.WebUI.Pages.Setup
{
    public class VendorModel : PageModel
    {
        public int SupplierId { get; set; }
        [Required]
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression(@"^[+0-9]*$", ErrorMessage = "Please enter a valid Phone number")]
        [MaxLength(15, ErrorMessage = "Please enter a valid Phone number")]
        [MinLength(11, ErrorMessage = "Please enter a valid Phone number")]
        [Phone(ErrorMessage = "Please enter a valid Phone number")]
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public void OnGet()
        {
        }
    }
}
