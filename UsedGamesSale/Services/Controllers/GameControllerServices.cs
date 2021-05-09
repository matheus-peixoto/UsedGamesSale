using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using UsedGamesSale.Models;
using UsedGamesSale.Services.Login;

namespace UsedGamesSale.Services.Controllers
{
    public class GameControllerServices
    {
        private readonly IConfiguration _configuration;
        private readonly SellerLoginManager _loginManager;

        public GameControllerServices(IConfiguration configuration, SellerLoginManager loginManager)
        {
            _configuration = configuration;
            _loginManager = loginManager;
        }

        public string GetImgsTempFolder() => $"{_configuration.GetValue<string>("Game:ImgsTempFolder")}/{_loginManager.GetUserId()}";

        public string GetImgsFolder() => $"{_configuration.GetValue<string>("Game:ImgsFolder")}";

        public int GetImgsPerGame() => _configuration.GetValue<int>("Game:ImgsPerGame");

        public bool IsNewImg(List<Image> imgs, string newImgName)
        {
            // If any image name is equal to the new image name, then it's not new 
            return !imgs.Any(i => i.Path.Split('/').Last() == newImgName);
        }
    }
}
