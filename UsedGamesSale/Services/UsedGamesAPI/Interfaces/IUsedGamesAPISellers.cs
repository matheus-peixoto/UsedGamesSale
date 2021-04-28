using System.Threading.Tasks;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUsedGamesAPISellers
    {
        public Task<UsedGamesAPISellerResponse> GetGamesAsync(int sellerId, string token);
    }
}
