using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Filters.Seller;
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
            if (!response.Success) return RedirectToAction(nameof(Error), "Home", new { area = "Seller" });
                
            return View(response.Games);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ConfigureSuccessMsg("Successfully logged")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            UsedGamesAPILoginResponse response = await _usedGamesAPISellers.LoginAsync(userDTO);
            if (!response.Success)
            {
                ViewData["MSG_E"] = response.Message;
                return View(userDTO);
            }

            _sellerLoginManager.Login(response.User, response.Token);
            User user = _sellerLoginManager.GetUser();
            return RedirectToAction(nameof(Index), "Home", new { area = "Seller" });
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }
}
