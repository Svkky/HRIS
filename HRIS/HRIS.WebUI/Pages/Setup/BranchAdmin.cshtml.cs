using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HRIS.WebUI.Pages.Setup
{
    public class BranchAdminModel : PageModel
    {
        public int BranchAdminId { get; set; }
        [RegularExpression("^[a-z A-Z]*$", ErrorMessage = "Only Alphabets allowed")]
        public string FirstName { get; set; }
        [RegularExpression("^[a-z A-Z]*$", ErrorMessage = "Only Alphabets allowed")]
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter a valid Phone number")]
        [RegularExpression("^[+0-9]*$", ErrorMessage = "Input a valid Phone Number.")]
        [MaxLength(15, ErrorMessage = "Lenght Exceeded..")]
        [MinLength(11, ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }

        public IActionResult OnGet(int branchId)
        {
            HttpContext.Session.SetString("branchIdCreate", branchId.ToString());
            return Page();
        }
    }
}
