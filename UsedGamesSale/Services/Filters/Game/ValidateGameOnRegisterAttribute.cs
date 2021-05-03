using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models.ViewModels;
using UsedGamesSale.Services.Image;
using UsedGamesSale.Services.UsedGamesAPI.Responses;
using UsedGamesSale.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using UsedGamesSale.Services.UsedGamesAPI;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using UsedGamesSale.Services.Login;

namespace UsedGamesSale.Services.Filters.Game
{
    public class ValidateGameOnRegisterAttribute : Attribute, IAsyncActionFilter
    {
        private readonly string _tempFolderKey;

        public ValidateGameOnRegisterAttribute(string tempFolderKey)
        {
            _tempFolderKey = tempFolderKey;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SellerLoginManager loginManager = (SellerLoginManager)context.HttpContext.RequestServices.GetService(typeof(SellerLoginManager));
            Controller controller = (Controller)context.Controller;
            int imgsPerGame = GetImgsPerGame(context);

            Models.Game game = (Models.Game)context.ActionArguments["game"];
            if (game.SellerId != loginManager.GetUserId()) context.Result = new BadRequestResult();

            string[] tempImgPaths = GetTempImgsPaths(controller);
            if (!context.ModelState.IsValid || tempImgPaths.Length < imgsPerGame)
            {
                if (tempImgPaths.Length < imgsPerGame) controller.ViewData["MSG_E"] = $"You need to have {imgsPerGame} images for the product";

                controller.ViewData["SellerId"] = loginManager.GetUserId();
                UsedGamesAPIPlatformResponse response = await GetPlatformsAsync(context);
                if (!response.Success) context.Result = new RedirectToActionResult("Error", "Home", new { area = "Seller" });

                SelectList platforms = new SelectList(response.Platforms, "Id", "Name");
                RegisterGameViewModel viewModel = new RegisterGameViewModel(game, platforms, imgsPerGame, tempImgPaths);
                context.Result = controller.View(viewModel);
            }
            else
            {
                await next();
            }
        }

        private string[] GetTempImgsPaths(Controller controller) 
        {
            ITempDataDictionary tempData = controller.TempData;
            return tempData.ContainsKey(_tempFolderKey) ? ImageHandler.GetAllTempImages(tempData.Peek(_tempFolderKey).ToString()) : new string[0]; 
        }

        private int GetImgsPerGame(ActionExecutingContext context)
        {
            IConfiguration configuration = (IConfiguration)context.HttpContext.RequestServices.GetService(typeof(IConfiguration));
            return configuration.GetValue<int>("Game:ImgsPerGame");
        }

        private async Task<UsedGamesAPIPlatformResponse> GetPlatformsAsync(ActionExecutingContext context)
        {
            UsedGamesAPIPlatforms usedGamesAPIPlatforms = (UsedGamesAPIPlatforms)context.HttpContext.RequestServices.GetService(typeof(UsedGamesAPIPlatforms));
            return await usedGamesAPIPlatforms.GetPlatformsAsync();
        }
    }
}
