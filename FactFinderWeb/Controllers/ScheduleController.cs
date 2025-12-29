using FactFinderWeb.BLL;
using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.ModelsView.AdminMV;
using FactFinderWeb.Services;
using FactFinderWeb.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FactFinderWeb.Controllers
{
    public class ScheduleController : Controller
    {
         private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AdminUserServices _AdminUserServices;
       
        private readonly UtilityHelperServices _utilService;
        private readonly IWebHostEnvironment _env;
        private readonly IViewRenderService _viewRenderService;
        private readonly PdfService _pdfService;
        private readonly JSONDataUtility _jsonData;
        public ScheduleController(ResellerBoyinawebFactFinderWebContext context, JSONDataUtility jSONDataUtility, UtilityHelperServices utilityHelperServices, IWebHostEnvironment env, IViewRenderService viewRenderService, PdfService pdfService)
        {
            _context = context;
          
            _utilService = utilityHelperServices;
            _env = env;
            _viewRenderService = viewRenderService;
            _pdfService = pdfService;
            _jsonData = jSONDataUtility;
        }


        // ✅ SCHEDULER / JOB ENDPOINT
        [HttpGet]
        [Route("schedule/job/generateassignedpdf")]
        public async Task<IActionResult> GenerateAssignedPdfJob()
        {
            var userProfile = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(p => p.ProfileStatus == "Assign" && p.Addby == null);

            if (userProfile == null)
                return Ok("No assigned profiles found.");

            var awakenData = await _jsonData.GetAwakenSection(userProfile.ProfileId);
            if (awakenData == null)
                return Ok("No data found.");

            string html = await _viewRenderService.RenderToStringAsync(
                "Schedule/assignedpdf",
                awakenData
            );

            byte[] pdfBytes = _pdfService.GeneratePdf(html, _env);


            var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PDFs");
            var fileName = "FF_"+ userProfile.PlanType + "_"+ userProfile.Name.Replace(" ","").Replace("  ", "").Replace("  ", "") + "_"+ userProfile.ProfileId +".pdf";
            var savePath = Path.Combine(baseFolder, fileName);
            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath);
                Console.WriteLine($"🗑️ Existing file deleted: {savePath}");
            }
            if (System.IO.File.Exists(savePath))
            {
                System.IO.File.Delete(savePath); // delete the old file
                Console.WriteLine($"🗑️ Existing file replaced: {savePath}");
            }

            await System.IO.File.WriteAllBytesAsync(savePath, pdfBytes);

            userProfile.PdfPath = "/PDFs/" + fileName;
            userProfile.PdfGeneratedOn = DateTime.Now;
            if (userProfile.ProfileStatus == "Assign")
            {
                userProfile.Addby = "pdf generted";
            }
            await _context.SaveChangesAsync();

            return Ok("PDF generated successfully");
        }

        // ✅ BROWSER VIEW (UI ONLY)
        [HttpGet]
        public async Task<IActionResult> assignedpdf()
        {
            var userProfile = await _context.TblffAwarenessProfileDetails
                .FirstOrDefaultAsync(p => p.ProfileStatus == "Assign" && p.Addby == null);

            if (userProfile == null)
                return View("NoAssignedProfile");

            var awakenData = await _jsonData.GetAwakenSection(userProfile.ProfileId);
            return View("AssignedPdf", awakenData);
        }


        [HttpGet]
        [Route("schedule/generatecompletedpdf/{id}")]
        public async Task<IActionResult> generatecompletedpdf(int id)
        {
            var today = DateTime.Today;

            var userProfile = _context.TblffAwarenessProfileDetails
                .Where(p =>
                    p.ProfileStatus == "Completed"
                    || (
                        p.ProfileStatus == "Pending"
                        && (
                            p.PdfGeneratedOn == null
                            || p.PdfGeneratedOn.Value.Date != today
                        )
                    )
                )
                // 1️⃣ Latest update first
                .OrderByDescending(p => p.UpdateDate)
                // 2️⃣ Pending first, then Completed
                .ThenBy(p => p.ProfileStatus == "Pending" ? 1 : 2)
                .FirstOrDefault();


            //p.Addby
            if (userProfile != null)
            {

               
                var planType = userProfile.PlanType?.ToLower();
                ViewData["planType"] = planType;

                // / pdf / downloads / 262


                var userIdString = HttpContext.Session.GetString("UserId");


                var user = await _context.TblffAwarenessProfileDetails
                    .FirstOrDefaultAsync(u => u.ProfileId == userProfile.ProfileId);

                if (user == null)
                    return NotFound(new { message = "Profile not found." });

                // ✅ Get full structured data (class, not JSON string)
                var awakenData = await _jsonData.GetAwakenSection(userProfile.ProfileId);

                // ✅ ASP.NET Core automatically serializes it to JSON in response

                return View(awakenData);
            }
            else
            {
                return Ok("No assigned profiles found.");
            }
        }


        [HttpPost]
        [Route("schedule/SaveGeneratedPdf/{profileId}")]
        public async Task<IActionResult> SaveGeneratedPdf(long profileId, IFormFile pdfFile)
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
                    System.IO.File.Delete(savePath);
                    Console.WriteLine($"🗑️ Existing file deleted: {savePath}");
                }
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

                var profile = await _context.TblffAwarenessProfileDetails.FirstOrDefaultAsync(u => u.ProfileId == profileId);
                if (profile != null)
                {
                    profile.PdfPath = "/PDFs/" + fileName;
                    profile.PdfGeneratedOn = DateTime.Now;
                    if (profile.ProfileStatus == "Assign")
                    {
                        profile.Addby = "pdf generted";
                    }
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


        [HttpGet]
        public IActionResult completedpdf()
        {
            var userProfile = _context.TblffAwarenessProfileDetails
                .FirstOrDefault(p => p.ProfileStatus == "Completed" || p.ProfileStatus == "Pending" && p.Addby == null);

            if (userProfile != null)
            {
                userProfile.Addby = "Completed file generated";
                _context.TblffAwarenessProfileDetails.Update(userProfile);
                _context.SaveChangesAsync();

                var planType = userProfile.PlanType?.ToLower();

                if (planType == "basic" || planType == "zero2one")
                {
                    return RedirectToAction(
                        "Downloads",          // Action name
                        "Pdf",                // Controller
                        new { id = userProfile.ProfileId } // Route values
                    );
                }
                else
                {
                    return RedirectToAction(
                        "Download",           // Action name
                        "Pdf",
                        new { id = userProfile.ProfileId }
                    );
                }

            }
            // / pdf / downloads / 262
            return Ok("No assigned profiles found.");
        }
    }
}
