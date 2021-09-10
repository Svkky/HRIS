using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.ComponentModel.DataAnnotations;

namespace HRIS.WebUI.Pages.Store
{
    public class ExpenditureModel : PageModel
    {
        public string Comment { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public DateTime ExpenditureDate { get; set; }
        public void OnGet()
        {
        }
    }
}
