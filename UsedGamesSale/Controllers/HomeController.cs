using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Controllers
{
    public class HomeController : Controller
    {
        private readonly UsedGamesAPIClients _usedGamesAPIClients;
        private readonly ClientLoginManager _clientLoginManager;

        public HomeController(UsedGamesAPIClients usedGamesAPIClients, ClientLoginManager clientLoginManager)
        {
            _usedGamesAPIClients = usedGamesAPIClients;
            _clientLoginManager = clientLoginManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ConfigureSuccessMsg("Successfully logged")]
        public async Task<IActionResult> Login(UserLoginDTO userDTO)
        {
            UsedGamesAPILoginResponse response = await _usedGamesAPIClients.LoginAsync(userDTO);
            if (!response.Success)
            {
                ViewData["MSG_E"] = response.Message;
                return View(userDTO);
            }

            _clientLoginManager.Login(response.User, response.Token);
            return RedirectToAction(nameof(Index));
        }
    }
}
