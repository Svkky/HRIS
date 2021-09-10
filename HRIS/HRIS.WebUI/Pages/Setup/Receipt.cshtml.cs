using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SelectPdf;

namespace HRIS.WebUI.Pages.Setup
{
    public class ReceiptModel : PageModel
    {
        public async Task<IActionResult>OnGet()
        {
            var mobileView = new HtmlToPdf();
            mobileView.Options.WebPageWidth = 480;

            var tabletView = new HtmlToPdf();
            tabletView.Options.WebPageWidth = 1024;

            var fullView = new HtmlToPdf();
            fullView.Options.WebPageWidth = 1920;

            var pdf = mobileView.ConvertUrl("http://localhost/HRIS.WebUI/Setup/Receipt");
            pdf.Append(tabletView.ConvertUrl("http://localhost/HRIS.WebUI/Setup/Receipt"));
            pdf.Append(fullView.ConvertUrl("http://localhost/HRIS.WebUI/Setup/Receipt"));

            var pdfBytes = pdf.Save();

            return File(pdfBytes, "application/pdf");
            
        }
    }
}
