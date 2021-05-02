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
        private readonly UsedGamesAPIPlatforms _usedGamesAPIPlatforms;
        private readonly int _imgsPerGame;
        private readonly IConfiguration _configuration;
        private SellerLoginManager _sellerLoginManager;
        private const string _tempFolderKey = "ImgTempFolder";

        public GameController(UsedGamesAPIPlatforms usedGamesAPIPlatforms, IConfiguration configuration, SellerLoginManager sellerLoginManager)
        {
            _usedGamesAPIPlatforms = usedGamesAPIPlatforms;
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
            RegisterGameViewModel viewModel = new RegisterGameViewModel()
            {
                Platforms = new SelectList(response.Platforms, "Id", "Name"),
                ImgsPerGame = _imgsPerGame
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateGameOnRegister(_tempFolderKey)]
        [ConfigureSuccessMsg("Game successfully registered")]
        public async Task<IActionResult> Register(Game game)
        {
            Result result = ImageHandler.MoveTempImgs(1, TempData[_tempFolderKey].ToString(), _configuration.GetValue<string>("Game:ImgFolder"));
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
            if (!recordResult.Success) return BadRequest();

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
