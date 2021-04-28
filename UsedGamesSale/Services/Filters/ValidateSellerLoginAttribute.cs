using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using UsedGamesSale.Services.Login;

namespace UsedGamesSale.Services.Filters
{
    public class ValidateSellerLoginAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            SellerLoginManager loginManager = context.HttpContext.RequestServices.GetService(typeof(SellerLoginManager)) as SellerLoginManager;
            if (!loginManager.IsLogged())
                context.Result = new RedirectToActionResult("Login", "Home", new {});
        }
    }
}
