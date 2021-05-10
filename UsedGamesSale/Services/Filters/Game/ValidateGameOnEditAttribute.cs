using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.ViewModels;
using UsedGamesSale.Services.Controllers;

namespace UsedGamesSale.Services.Filters.Game
{
    public class ValidateGameOnEditAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Controller controller = (Controller)context.Controller;
            GameControllerServices controllerServices = (GameControllerServices)context.HttpContext.RequestServices.GetService(typeof(GameControllerServices));
            if (!context.ModelState.IsValid)
            {
                GameViewModel viewModel = await controllerServices.GetGameViewModelForEditAsync();
                viewModel.Game = (Models.Game)context.ActionArguments["Game"];
                List<Image> images = await controllerServices.GetImagesAsync(viewModel.Game.Id);
                if (images is null)
                    context.Result = new RedirectToActionResult("Error", "Home", new { area = "Seller" });
                else
                {
                    viewModel.Game.Images = images;
                    context.Result = new ViewResult() { ViewData = new ViewDataDictionary(controller.ViewData) { Model = viewModel } };
                }
            }
            else
            {
                await next();
            }
        }
    }
}
