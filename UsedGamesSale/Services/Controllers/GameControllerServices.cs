using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Services.ImageFilter;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.Controllers
{
    public class GameControllerServices
    {
        private readonly UsedGamesAPIGames _usedGamesAPIGames;
        private readonly IConfiguration _configuration;
        private readonly SellerLoginManager _loginManager;

        public GameControllerServices(UsedGamesAPIGames usedGamesAPIGames, IConfiguration configuration, SellerLoginManager loginManager)
        {
            _usedGamesAPIGames = usedGamesAPIGames;
            _configuration = configuration;
            _loginManager = loginManager;
        }

        public async Task<Result> RegisterGameAsync(Game game)
        {
            Result result = new Result();
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.CreateAsync(game, _loginManager.GetUserToken());
            if (!response.Success) return result;

            RecordResult recordResult = ImageHandler.MoveTempImgs(response.Game.Id, GetImgsTempFolder(), GetImgsFolder());
            if (!recordResult.Success) return result;

            response = await _usedGamesAPIGames.CreateImagesAsync(response.Game.Id, recordResult.Paths, _loginManager.GetUserToken());
            if (!response.Success) return result;

            ImageHandler.DeleteImgFolder(GetImgsTempFolder());
            result.Success = true;
            return result;
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
