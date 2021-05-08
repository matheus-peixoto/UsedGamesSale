using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.ViewModels;
using UsedGamesSale.Services.Controllers;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Filters.Game;
using UsedGamesSale.Services.Filters.Seller;
using UsedGamesSale.Services.Image;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Areas.Seller.Controllers
{
    [Area("Seller")]
    [ValidateSellerLogin]
    public class GameController : Controller
    {
        private readonly UsedGamesAPIGames _usedGamesAPIGames;
        private readonly UsedGamesAPIPlatforms _usedGamesAPIPlatforms;
        private SellerLoginManager _sellerLoginManager;
        private GameControllerServices _controllerServices;

        public GameController(UsedGamesAPIGames usedGamesAPIGames, UsedGamesAPIPlatforms usedGamesAPIPlatforms, SellerLoginManager sellerLoginManager, GameControllerServices services)
        {
            _usedGamesAPIPlatforms = usedGamesAPIPlatforms;
            _usedGamesAPIGames = usedGamesAPIGames;
            _controllerServices = services;
            _sellerLoginManager = sellerLoginManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            ImageHandler.DeleteImgFolder(_controllerServices.GetImgsTempFolder());

            UsedGamesAPIPlatformResponse response = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            GameViewModel viewModel = new GameViewModel
            (
                platforms: new SelectList(response.Platforms, "Id", "Name"), imgsPerGame: _controllerServices.GetImgsPerGame(), sellerId: _sellerLoginManager.GetUserId()
            );
            return View(viewModel);
        }

        [HttpPost]
        [ValidateGameOnRegister]
        [AddGamePostDate]
        [ConfigureSuccessMsg("Game successfully registered")]
        public async Task<IActionResult> Register(Game game)
        {
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.CreateAsync(game, _sellerLoginManager.GetUserToken());
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            RecordResult result = ImageHandler.MoveTempImgs(response.Game.Id, _controllerServices.GetImgsTempFolder(), _controllerServices.GetImgsFolder());
            if (!result.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            response = await _usedGamesAPIGames.CreateImagesAsync(response.Game.Id, result.Paths, _sellerLoginManager.GetUserToken());
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            ImageHandler.DeleteImgFolder(_controllerServices.GetImgsTempFolder());

            return RedirectToAction("Index", "Home", new { area = "Seller" });
        }

        [HttpPost]
        public IActionResult UploadTempImage([FromForm] IFormFile img)
        {
            RecordResult recordResult = ImageHandler.Record(_controllerServices.GetImgsTempFolder(), img);
            if (!recordResult.Success) return BadRequest(new { errorMsg = recordResult.ErrorMessage });

            return Ok(new { imgPath = recordResult.Path });
        }

        [HttpGet]
        public IActionResult DeleteTempImage([FromQuery] string imgPath)
        {
            Result result = ImageHandler.Delete(imgPath);
            if (!result.Success) return BadRequest(result.ErrorMessage);

            return Ok();
        }
    }
}
