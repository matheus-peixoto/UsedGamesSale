using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;

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
        public IActionResult Index()
        {
            return View();
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

            UsedGamesAPIResponse response = await _usedGamesAPISellers.LoginAsync(userDTO);
            if (!response.Success)
            {
                ViewData["MSG_E"] = response.Message;
                return View(userDTO);
            }

            _sellerLoginManager.Login(response.User, response.Token);

            TempData.Remove("MSG_S");
            TempData.Add("MSG_S", "Successfully logged");
            return RedirectToAction(nameof(Index));
        }
    }
}
