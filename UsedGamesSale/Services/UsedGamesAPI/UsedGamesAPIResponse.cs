using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIResponse
    {
        public string Message { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public bool Success { get; set; }
    }
}
