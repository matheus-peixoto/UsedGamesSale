using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;

namespace UsedGamesSale.Areas.Seller.Controllers
{
    [Area("Seller")]
    public class HomeController : Controller
    {
        private readonly UsedGamesAPISellers _usedGamesAPISellers;
        private readonly SellerLoginManager _sellerLoginManager;

        public HomeController(UsedGamesAPISellers usedGamesAPISellers, SellerLoginManager sellerLoginManager)
        {
            _usedGamesAPISellers = usedGamesAPISellers;
            _sellerLoginManager = sellerLoginManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
