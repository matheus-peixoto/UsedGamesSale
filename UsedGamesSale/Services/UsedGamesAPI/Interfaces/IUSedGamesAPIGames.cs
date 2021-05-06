using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUSedGamesAPIGames
    {
        public Task<UsedGamesAPIGameResponse> CreateAsync(Game game, string token);
    }
}
