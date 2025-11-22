using Microsoft.AspNetCore.Mvc;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
//Comprehensive
namespace FactFinderWeb.Controllers
{
    //[Route("plan")]
    public class BasicController : Controller
    {
        private readonly ResellerBoyinawebFactFinderWebContext _context;
        private readonly AwarenessServices _AwarenessServices;
        private readonly WingsServices _WingsServices;
        private readonly KnowledgeThatMattersServices _KnowledgeThatMattersServices;
        private readonly long _userID;
        private readonly HttpContext _httpContext;
        private readonly ExecutionServices _executionServices;
        private readonly InvestServices _investServices;
        int updateRows = 0;

        public BasicController(ResellerBoyinawebFactFinderWebContext context, AwarenessServices awarenessServices, IHttpContextAccessor httpContextAccessor, WingsServices wingsServices, KnowledgeThatMattersServices knowledgeThatMattersServices, ExecutionServices executionServices, InvestServices investServices)
        {
            _context = context;
            _AwarenessServices = awarenessServices;
            _WingsServices = wingsServices;
            _KnowledgeThatMattersServices = knowledgeThatMattersServices;

            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            _userID = Convert.ToInt64(userIdStr);
            _executionServices = executionServices;
            _investServices = investServices;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.IsNullOrEmpty(_userID.ToString()) || _userID == 0)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                { "controller", "User" },
                { "action", "Login" }
            });
            }

            base.OnActionExecuting(context);
        }

         

    }
}
