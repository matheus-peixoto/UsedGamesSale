using System.Collections.Generic;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUSedGamesAPIGames
    {
        public Task<UsedGamesAPIGameResponse> GetAsync(int id, string token);
        public Task<UsedGamesAPIGameResponse> CreateAsync(Game game, string token);
        public Task<UsedGamesAPIGameResponse> CreateImagesAsync(int id, List<string> imgPaths, string token)
    }
}
