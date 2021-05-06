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
        private readonly int _imgsPerGame;
        private readonly IConfiguration _configuration;
        private SellerLoginManager _sellerLoginManager;
        private const string _tempFolderKey = "ImgTempFolder";

        public GameController(UsedGamesAPIGames usedGamesAPIGames, UsedGamesAPIPlatforms usedGamesAPIPlatforms, IConfiguration configuration, SellerLoginManager sellerLoginManager)
        {
            _usedGamesAPIPlatforms = usedGamesAPIPlatforms;
            _usedGamesAPIGames = usedGamesAPIGames;
            _configuration = configuration;
            _imgsPerGame = configuration.GetValue<int>("Game:ImgsPerGame");
            _sellerLoginManager = sellerLoginManager;
        }

        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (TempData.ContainsKey(_tempFolderKey))
                ImageHandler.DeleteImgFolder(TempData[_tempFolderKey].ToString());

            UsedGamesAPIPlatformResponse response = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            GameViewModel viewModel = new GameViewModel
            (
                platforms: new SelectList(response.Platforms, "Id", "Name"), imgsPerGame: _imgsPerGame, sellerId: _sellerLoginManager.GetUserId()
            );
            return View(viewModel);
        }

        [HttpPost]
        [ValidateGameOnRegister(_tempFolderKey)]
        [AddGamePostDate]
        [ConfigureSuccessMsg("Game successfully registered")]
        public async Task<IActionResult> Register(Game game)
        {
            string[] relativePaths = ImageHandler.GetAllTempImageRelativePaths(TempData[_tempFolderKey].ToString());
            game.Images = new List<Image>();
            foreach (var relativePath in relativePaths)
            {
                game.Images.Add(new Image(relativePath));
            }
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.CreateAsync(game, _sellerLoginManager.GetUserToken());
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            Result result = ImageHandler.MoveTempImgs(response.Game.Id, TempData[_tempFolderKey].ToString(), _configuration.GetValue<string>("Game:ImgFolder"));
            if (!result.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            return RedirectToAction("Index", "Home", new { area = "Seller" });
        }

        [HttpPost]
        public IActionResult UploadTempImage([FromForm] IFormFile img)
        {
            string relativeTempPath;
            if (!TempData.ContainsKey("imgTempFolder"))
            {
                relativeTempPath = $"{_configuration.GetValue<string>("Game:ImgTempFolder")}/{_sellerLoginManager.GetUserId()}";
                TempData["imgTempFolder"] = relativeTempPath;
            }

            relativeTempPath = TempData.Peek("imgTempFolder").ToString();
            RecordResult recordResult = ImageHandler.Record(relativeTempPath, img);
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
