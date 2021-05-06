using System.Collections.Generic;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPIImageResponse : UsedGamesAPIResponse
    {
        public List<Models.Image> Images { get; set; }
    }
}
