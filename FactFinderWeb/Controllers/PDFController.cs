using FactFinderWeb.BLL;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace FactFinderWeb.Controllers
{
  
    public class PDFController : Controller
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AdminUserServices _AdminUserServices;
        private readonly JSONDataUtility _jsonData;
        private readonly UtilityHelperServices _utilService;
        private readonly IWebHostEnvironment _env;

       
        public PDFController(ResellerBoyinawebFactFinderWebContext context, JSONDataUtility jSONDataUtility, UtilityHelperServices utilityHelperServices, IWebHostEnvironment env)
        {
            _context = context;
            _jsonData = jSONDataUtility;
            _utilService = utilityHelperServices;
            _env = env;
        }


        [HttpGet]
        [Route("pdf/DownloadProfile/{profileId}")]
        public async Task<IActionResult> DownloadProfile(long profileId)
        {
            string weburl = _utilService.webAppURL();

            var url = weburl+"/PDF/userprofileDownload/"+ profileId;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string path = "";
                    var html = await response.Content.ReadAsStringAsync();
                    var profile = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(u => u.ProfileId == profileId);
                    if (profile != null)
                    {
                        path = profile.PdfPath;
                    }
                    
                    return Redirect(weburl +"/"+ path);
                   
                    // Show HTML directly in browser
                   
                }
                else
                {
                    return Content($"❌ Failed to call API: {response.StatusCode}");
                }
            }

           

        }
        [HttpGet]
        [Route("pdf/download/{profileId}")]
        public async Task<IActionResult> UserProfile(long profileId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");


            var user = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(u => u.ProfileId == profileId);

            if (user == null)
                return NotFound(new { message = "Profile not found." });

            // ✅ Get full structured data (class, not JSON string)
            var awakenData = await _jsonData.GetAwakenSection(profileId);

            // ✅ ASP.NET Core automatically serializes it to JSON in response
            return View(awakenData);
        }
        [HttpGet]
        [Route("pdf/downloads/{profileId}")]
        public async Task<IActionResult> UserProfileForBasicZero(long profileId)
        {
            var userIdString = HttpContext.Session.GetString("UserId");


            var user = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(u => u.ProfileId == profileId);

            if (user == null)
                return NotFound(new { message = "Profile not found." });

            // ✅ Get full structured data (class, not JSON string)
            var awakenData = await _jsonData.GetAwakenSection(profileId);

            // ✅ ASP.NET Core automatically serializes it to JSON in response
            return View(awakenData);
        }

        

        [HttpPost]
        [Route("pdf/SaveGeneratedPdf/{profileId}")]
        public async Task<IActionResult> SaveGeneratedPdf(long profileId,IFormFile pdfFile)
        {
            try
            {
                if (pdfFile == null || pdfFile.Length == 0)
                    return BadRequest("No PDF file uploaded.");

                // Extract client name from file name (optional)
                // Example filename: FF_Basic_Kamlesh_123456.pdf
                var fileName = Path.GetFileName(pdfFile.FileName);
                // var clientName = "General";


                // Define base folder: /wwwroot/PDFs/
                var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDFs");

                // Define full file path
                var savePath = Path.Combine(baseFolder, fileName);
                if (System.IO.File.Exists(savePath))
                {
                    System.IO.File.Delete(savePath); // delete the old file
                    Console.WriteLine($"🗑️ Existing file replaced: {savePath}");
                }

                // Save the uploaded PDF
                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    await pdfFile.CopyToAsync(stream);
                }

                Console.WriteLine($"✅ PDF saved successfully: {savePath}");

                var profile = await _context.TblffAwarenessProfileDetails .FirstOrDefaultAsync(u => u.ProfileId == profileId);
                if (profile != null)
                {
                    profile.PdfPath = "/PDFs/" + fileName;
                }
                _context.TblffAwarenessProfileDetails.Update(profile);
                await _context.SaveChangesAsync();
                // Return JSON response
                return Ok(new
                {
                    success = true,
                    message = "PDF saved successfully.",
                    url = $"/PDFs/" + fileName
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error saving PDF: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Server error while saving PDF.", error = ex.Message });
            }
        }

    }
}
