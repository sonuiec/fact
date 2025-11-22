using DinkToPdf;
using DinkToPdf.Contracts;
using FactFinderWeb.BLL;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FactFinderWeb.Controllers
{
    public class PDFNewController : Controller
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly JSONDataUtility _jsonData;
        private readonly UtilityHelperServices _utilService;
        private readonly IWebHostEnvironment _env;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IConverter _converter;

        public PDFNewController(
            ResellerBoyinawebFactFinderWebContext context,
            JSONDataUtility jsonDataUtility,
            UtilityHelperServices utilityHelperServices,
            IWebHostEnvironment env,
            ICompositeViewEngine viewEngine,
            ITempDataProvider tempDataProvider)
        {
            _context = context;
            _jsonData = jsonDataUtility;
            _utilService = utilityHelperServices;
            _env = env;
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _converter = new SynchronizedConverter(new PdfTools());
        }

        [HttpGet("PDFNew/UserProfile/{profileId}")]
        public async Task<IActionResult> UserProfile(long profileId)
        {
            var user = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(u => u.ProfileId == profileId);
            if (user == null)
                return NotFound("Profile not found.");

            var awakenData = await _jsonData.GetAwakenSection(profileId);
            return View("UserProfile", awakenData);
        }

        [HttpGet("pdfnew/generate/{profileId}")]
        public async Task<IActionResult> GeneratePdf(long profileId)
        {
            // 1️⃣ Fetch profile from DB
            var user = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(u => u.ProfileId == profileId);

            if (user == null)
                return NotFound("Profile not found.");

            // 2️⃣ Ensure output folder exists
            string folder = Path.Combine(_env.WebRootPath, "GeneratedPDFs");
            Directory.CreateDirectory(folder);

            // 3️⃣ Define the output PDF file path
            string pdfPath = Path.Combine(folder, $"UserProfile_{profileId}.pdf");

            // 4️⃣ Delete existing file (avoid old version conflicts)
            if (System.IO.File.Exists(pdfPath))
                System.IO.File.Delete(pdfPath);

            // 5️⃣ Generate your UserProfile page URL (the one you want to convert)
            var pageUrl = $"{Request.Scheme}://{Request.Host}/pdf/download/{profileId}";
            Console.WriteLine($"🔗 Generating PDF from: {pageUrl}");

            // 6️⃣ Configure DinkToPdf
            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = new GlobalSettings
                {
                    PaperSize = PaperKind.A4,
                    Orientation = Orientation.Portrait,
                    Out = pdfPath
                },
                Objects = {
            new ObjectSettings {
                Page = pageUrl,  // 👈 loads your Razor page
                WebSettings = {
                    DefaultEncoding = "utf-8",
                    EnableJavascript = false,  // ⚡ faster
                    LoadImages = true
                },
                FooterSettings = { Right = "Page [page] of [toPage]" }
            }
        }
            };

            // 7️⃣ Generate the PDF
            try
            {
                converter.Convert(doc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ PDF generation failed: {ex.Message}");
                return StatusCode(500, $"PDF generation failed: {ex.Message}");
            }

            // 8️⃣ Wait until the file actually exists (wkhtmltopdf is async under the hood)
            var sw = Stopwatch.StartNew();
            while (!System.IO.File.Exists(pdfPath) && sw.Elapsed < TimeSpan.FromSeconds(10))
            {
                await Task.Delay(200);
            }

            if (!System.IO.File.Exists(pdfPath))
            {
                return StatusCode(500, "❌ PDF file was not generated in time.");
            }

            // 9️⃣ Optional: update database path
            user.PdfPath = $"/GeneratedPDFs/UserProfile_{profileId}.pdf";
            _context.Update(user);
            await _context.SaveChangesAsync();

            // 🔟 Return the generated PDF file
            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(pdfPath);
            return File(fileBytes, "application/pdf", $"UserProfile_{profileId}.pdf");
        }



        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
            ViewData.Model = model;
            using var sw = new StringWriter();

            var viewResult = _viewEngine.FindView(ControllerContext, viewName, false);
            if (viewResult.View == null)
                throw new ArgumentNullException($"View '{viewName}' not found.");

            var viewContext = new ViewContext(
                ControllerContext,
                viewResult.View,
                ViewData,
                new TempDataDictionary(HttpContext, _tempDataProvider),
                sw,
                new HtmlHelperOptions()
            );

            await viewResult.View.RenderAsync(viewContext);
            return sw.ToString();
        }
    }
}
