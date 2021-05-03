using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace UsedGamesSale.Services.Filters.Game
{
    public class AddGamePostDateAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            Models.Game game = (Models.Game)context.ActionArguments["game"];
            game.PostDate = DateTime.Now;
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
