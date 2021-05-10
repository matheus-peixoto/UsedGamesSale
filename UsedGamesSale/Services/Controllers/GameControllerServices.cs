using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.GameDTOs;
using UsedGamesSale.Models.ViewModels;
using UsedGamesSale.Services.ImageFilter;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.Controllers
{
    public class GameControllerServices
    {
        private readonly UsedGamesAPIGames _usedGamesAPIGames;
        private readonly UsedGamesAPIPlatforms _usedGamesAPIPlatforms;
        private readonly IConfiguration _configuration;
        private readonly SellerLoginManager _loginManager;

        public GameControllerServices(UsedGamesAPIGames usedGamesAPIGames, UsedGamesAPIPlatforms usedGamesAPIPlatforms, IConfiguration configuration, SellerLoginManager loginManager)
        {
            _usedGamesAPIGames = usedGamesAPIGames;
            _usedGamesAPIPlatforms = usedGamesAPIPlatforms;
            _configuration = configuration;
            _loginManager = loginManager;
        }

        public async Task<List<Image>> GetImagesAsync(int id)
        {
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.GetImagesAsync(id, _loginManager.GetUserToken());
            if (!response.Success) return null;

            return response.Images;
        }

        public async Task<GameViewModel> GetGameViewModelForRegisterAsync()
        {
            UsedGamesAPIPlatformResponse response = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!response.Success) return null;

            GameViewModel viewModel = new GameViewModel
            (
                platforms: new SelectList(response.Platforms, "Id", "Name"), imgsPerGame: GetImgsPerGame(), sellerId: _loginManager.GetUserId()
            );
            return viewModel;
        }

        public async Task<GameViewModel> GetGameViewModelForEditAsync()
        {
            UsedGamesAPIPlatformResponse platformResponse = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!platformResponse.Success) return null;

            GameViewModel viewModel = new GameViewModel
            (
                platforms: new SelectList(platformResponse.Platforms, "Id", "Name"),
                imgsPerGame: GetImgsPerGame(), sellerId: _loginManager.GetUserId()
            );

            return viewModel;
        }

        public async Task<GameViewModel> GetGameViewModelForEditAsync(int id)
        {
            UsedGamesAPIGameResponse gameResponse = await _usedGamesAPIGames.GetAsync(id, _loginManager.GetUserToken());
            if (!gameResponse.Success) return null;

            UsedGamesAPIPlatformResponse platformResponse = await _usedGamesAPIPlatforms.GetPlatformsAsync();
            if (!platformResponse.Success) return null;

            GameViewModel viewModel = new GameViewModel
            (
                game: gameResponse.Game, platforms: new SelectList(platformResponse.Platforms, "Id", "Name"),
                imgsPerGame: GetImgsPerGame(), sellerId: _loginManager.GetUserId()
            );

            return viewModel;
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

        public async Task<Result> EditGameAsync(Game game)
        {
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.UpdateAsync(game, _loginManager.GetUserToken());
            Result result = new Result(response.Success);
            return result;
        }

        public async Task<RecordResult> ChangeImageAsync(ChangeImagedDto gameDto)
        {
            RecordResult recordResult = new RecordResult();
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.GetImagesAsync(gameDto.GameId, _loginManager.GetUserToken());
            if (!response.Success) return recordResult;

            if (!IsNewImg(response.Images, gameDto.ImgFile.FileName)) {
                recordResult.ErrorMessage = "This is not a new image";
                return recordResult; 
            }

            recordResult = ImageHandler.Change($"{GetImgsFolder()}/{gameDto.GameId}", gameDto.OldImgRelativePath, gameDto.ImgFile);
            if (!recordResult.Success) return recordResult;

            Image img = new Image(gameDto.ImgId, recordResult.Path, gameDto.GameId);
            response = await _usedGamesAPIGames.UpdateImageAsync(img, _loginManager.GetUserToken());

            recordResult.Success = response.Success;
            return recordResult;
        }

        public async Task<Result> DeleteGameAsync(int id)
        {
            UsedGamesAPIGameResponse response = await _usedGamesAPIGames.DeleteAsync(id, _loginManager.GetUserToken());
            Result result = new Result(response.Success);

            if (result.Success) ImageHandler.DeleteImgFolder($"{GetImgsFolder()}/{id}");

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
