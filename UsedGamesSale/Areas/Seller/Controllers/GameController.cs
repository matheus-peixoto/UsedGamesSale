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
using UsedGamesSale.Services.Filters;
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
            if (TempData.ContainsKey("imgTempFolder"))
                ImageHandler.DeleteImgFolder(TempData["imgTempFolder"].ToString());

           UsedGamesAPIPlatformResponse response = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            ViewData["Platforms"] = new SelectList(response.Platforms, "Id", "Name");
            ViewData["ImgsPerGame"] = _imgsPerGame;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Game game)
        {
            UsedGamesAPIPlatformResponse response = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!response.Success) return RedirectToAction("Error", "Home", new { area = "Seller" });

            ViewData["Platforms"] = new SelectList(response.Platforms, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult UploadTempImage(IFormFile img)
        {
            string relativeTempPath;
            if (!TempData.ContainsKey("imgTempFolder"))
            {
                relativeTempPath = $"{_configuration.GetValue<string>("Game:ImgTempFolder")}/{Guid.NewGuid()}";
                TempData["imgTempFolder"] = relativeTempPath;
            }
            relativeTempPath = TempData.Peek("imgTempFolder").ToString();

            RecordResult recordResult = ImageHandler.Record(relativeTempPath, img);
            if (!recordResult.Success) return BadRequest();

            return Ok(new { imgPath = recordResult.Path });
        }
    }
}
