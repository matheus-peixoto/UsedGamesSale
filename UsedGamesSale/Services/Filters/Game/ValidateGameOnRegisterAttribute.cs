using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using UsedGamesSale.Models.ViewModels;
using UsedGamesSale.Services.ImageFilter;
using UsedGamesSale.Services.UsedGamesAPI.Responses;
using Microsoft.AspNetCore.Mvc.Rendering;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.Controllers;

namespace UsedGamesSale.Services.Filters.Game
{
    public class ValidateGameOnRegisterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SellerLoginManager loginManager = (SellerLoginManager)context.HttpContext.RequestServices.GetService(typeof(SellerLoginManager));
            Controller controller = (Controller)context.Controller;
            GameControllerServices controllerServices = (GameControllerServices)context.HttpContext.RequestServices.GetService(typeof(GameControllerServices));
            int imgsPerGame = controllerServices.GetImgsPerGame();

            Models.Game game = (Models.Game)context.ActionArguments["game"];
            if (game.SellerId != loginManager.GetUserId()) context.Result = new BadRequestResult();

            string[] tempImgPaths = ImageHandler.GetAllTempImageRelativePaths(controllerServices.GetImgsTempFolder());
            if (!context.ModelState.IsValid || tempImgPaths.Length < imgsPerGame)
            {
                if (tempImgPaths.Length < imgsPerGame) controller.ViewData["MSG_E"] = $"You need to have {imgsPerGame} images for the product";

                controller.ViewData["SellerId"] = loginManager.GetUserId();
                UsedGamesAPIPlatformResponse response = await GetPlatformsAsync(context);
                if (!response.Success) context.Result = new RedirectToActionResult("Error", "Home", new { area = "Seller" });

                SelectList platforms = new SelectList(response.Platforms, "Id", "Name");
                GameViewModel viewModel = new GameViewModel(game, platforms, imgsPerGame, tempImgPaths, loginManager.GetUserId());
                context.Result = controller.View(viewModel);
            }
            else
            {
                await next();
            }
        }

        private async Task<UsedGamesAPIPlatformResponse> GetPlatformsAsync(ActionExecutingContext context)
        {
            UsedGamesAPIPlatforms usedGamesAPIPlatforms = (UsedGamesAPIPlatforms)context.HttpContext.RequestServices.GetService(typeof(UsedGamesAPIPlatforms));
            return await usedGamesAPIPlatforms.GetPlatformsAsync();
        }
    }
}
