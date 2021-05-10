using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPILoginResponse : UsedGamesAPIResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public User User { get; set; }

        public UsedGamesAPILoginResponse(bool success)
        {
            Success = success;
        }
    }
}
