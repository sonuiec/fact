using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FactFinderWeb.Services
{
    public class SessionCheckFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var adminUserId = httpContext.Session.GetString("AdminUserId");

            // If not logged in, redirect
            if (string.IsNullOrEmpty(adminUserId))
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }
    }
}
