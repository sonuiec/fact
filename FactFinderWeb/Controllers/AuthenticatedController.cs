using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FactFinderWeb.Controllers
{

    public class AuthenticatedController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var adminUserId = HttpContext.Session.GetString("AdminUserId");

            // Skip for Login, Logout, ForgotPassword actions
            var actionName = context.ActionDescriptor.RouteValues["action"]?.ToLower();
            if (string.IsNullOrEmpty(adminUserId) &&
                actionName != "login" &&
                actionName != "logout" &&
                actionName != "forgotpassword")
            {
                context.Result = RedirectToAction("Login", "Admin");
            }

            base.OnActionExecuting(context);
        }
    }

}
