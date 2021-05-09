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
using UsedGamesSale.Services.ImageFilter;
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
        public async Task<IActionResult> Register([FromForm] Game game)
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

        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
            ImageHandler.DeleteImgFolder(_controllerServices.GetImgsTempFolder());

            UsedGamesAPIGameResponse gameResponse = await _usedGamesAPIGames.GetAsync(id, _sellerLoginManager.GetUserToken());
            if (!gameResponse.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            UsedGamesAPIPlatformResponse platformResponse = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!platformResponse.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            GameViewModel viewModel = new GameViewModel
            (
                game: gameResponse.Game, platforms: new SelectList(platformResponse.Platforms, "Id", "Name"),
                imgsPerGame: _controllerServices.GetImgsPerGame(), sellerId: _sellerLoginManager.GetUserId()
            );
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] Game game)
        {
            return BadRequest();
        }

        [HttpPost]
        public IActionResult UploadTempImage([FromForm] IFormFile img)
        {
            RecordResult recordResult = ImageHandler.Record(_controllerServices.GetImgsTempFolder(), img);
            if (!recordResult.Success) return BadRequest(new { errorMsg = recordResult.ErrorMessage });
            return Ok(new { imgPath = recordResult.Path });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeImage([FromForm] GameViewModel viewModel)
        {
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.GetImagesAsync(viewModel.GameId, _sellerLoginManager.GetUserToken());
            if (!response.Success) return BadRequest();

            if (!_controllerServices.IsNewImg(response.Images, viewModel.ImgFile.FileName)) return BadRequest(new { ErrorMessage = "This is not a new image" });

            RecordResult recordResult = ImageHandler.Change($"{_controllerServices.GetImgsFolder()}/{viewModel.GameId}", viewModel.OldImgRelativePath, viewModel.ImgFile);
            if (!recordResult.Success) return BadRequest(new { errorMsg = recordResult.ErrorMessage });

            Image img = new Image(viewModel.ImgId, recordResult.Path, viewModel.GameId);
            response = await _usedGamesAPIGames.UpdateImageAsync(img, _sellerLoginManager.GetUserToken());
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

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
