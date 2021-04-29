using System.Threading.Tasks;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUsedGamesAPISellers
    {
        public Task<UsedGamesAPISellerResponse> GetGamesAsync(int sellerId, string token);
    }
}
