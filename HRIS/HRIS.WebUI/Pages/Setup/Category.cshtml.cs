using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace HRIS.WebUI.Pages.Setup
{
    public class CategoryModel : PageModel
    {
        public int CategoryId { get; set; }
        [Required]
        public string Description { get; set; }
        public void OnGet()
        {
        }
    }
}
