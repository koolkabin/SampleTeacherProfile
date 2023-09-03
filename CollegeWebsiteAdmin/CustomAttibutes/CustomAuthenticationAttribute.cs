using CollegeWebsiteAdmin.Extensions;
using CollegeWebsiteAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CollegeWebsiteAdmin.CustomAttibutes
{
    public class CustomAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Teacher SessionValue = context.HttpContext.Session.Get<Teacher>("LoggedInUser");

            if (SessionValue == null)
            {
                //
                context.Result = new RedirectToActionResult("Login", "Home", new { });
            }
        }
    }
}
