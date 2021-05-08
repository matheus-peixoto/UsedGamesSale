using Microsoft.Extensions.Configuration;
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
    }
}
