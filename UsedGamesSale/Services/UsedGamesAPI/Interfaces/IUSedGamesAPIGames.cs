using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUSedGamesAPIGames
    {
        public Task<UsedGamesAPIGameResponse> CreateGameAsync(Game game, string token);
    }
}
