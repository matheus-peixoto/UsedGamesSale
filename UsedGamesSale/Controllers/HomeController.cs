using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UsedGamesSale.Models;
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

        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}
