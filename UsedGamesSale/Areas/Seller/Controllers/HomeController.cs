using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class HomeController : Controller
    {
        private readonly UsedGamesAPISellers _usedGamesAPISellers;
        private SellerLoginManager _sellerLoginManager;

        public HomeController(UsedGamesAPISellers usedGamesAPISellers, SellerLoginManager sellerLoginManager)
        {
            _usedGamesAPISellers = usedGamesAPISellers;
            _sellerLoginManager = sellerLoginManager;
        }

        [HttpGet]
        [ValidateSellerLogin]
        public async Task<IActionResult> Index()
        {
            UsedGamesAPISellerResponse response = await _usedGamesAPISellers.GetGamesAsync(_sellerLoginManager.GetUserId(), _sellerLoginManager.GetUserToken());
            if (!response.Success)
            {
                if (response.IsUnauthorized)
                    return RedirectToAction(nameof(Login), "Home", new { area = "Seller" });
                else
                    return RedirectToAction(nameof(Error), "Home", new { area = "Seller" });
            }
                
            return View(response.Games);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid) return View(userDTO);

            UsedGamesAPILoginResponse response = await _usedGamesAPISellers.LoginAsync(userDTO);
            if (!response.Success)
            {
                ViewData["MSG_E"] = response.Message;
                return View(userDTO);
            }

            _sellerLoginManager.Login(response.User, response.Token);
            User user = _sellerLoginManager.GetUser();
            TempData.Remove("MSG_S");
            TempData.Add("MSG_S", "Successfully logged");
            return RedirectToAction(nameof(Index), "Home", new { area = "Seller" });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
