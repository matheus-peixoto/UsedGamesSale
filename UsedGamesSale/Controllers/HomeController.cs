using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI;

namespace UsedGamesSale.Controllers
{
    public class HomeController : Controller
    {
        private UsedGamesAPIClients _usedGamesAPIClients;

        public HomeController(UsedGamesAPIClients usedGamesAPIClients)
        {
            _usedGamesAPIClients = usedGamesAPIClients;
        }

        [HttpGet]
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

            UsedGamesAPIResponse response = await _usedGamesAPIClients.LoginAsync(userDTO);
            if (!response.Success)
            {
                ViewData["MSG_E"] = response.Message;
                return View(userDTO);
            }

            TempData.Remove("ClientToken");
            TempData.Add("ClientToken", response.Token);
            TempData.Remove("MSG_S");
            TempData.Add("MSG_S", "Successfully logged");
            return RedirectToAction(nameof(Index));
        }
    }
}
