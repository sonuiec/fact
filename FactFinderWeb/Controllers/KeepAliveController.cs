using Microsoft.AspNetCore.Mvc;

namespace FactFinderWeb.Controllers
{
    public class KeepAliveController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            // Check session and update timestamp to keep it alive
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UserId")))
            {
                HttpContext.Session.SetString("LastPing", DateTime.Now.ToString());
            }

            return Ok();
        }
    }
}
