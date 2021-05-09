using System;
using System.Collections.Generic;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPIGameResponse : UsedGamesAPIResponse
    {
        public Game Game { get; set; }
        public List<Game> Games { get; set; }
        public List<Image> Images { get; set; }

        public UsedGamesAPIGameResponse() { }

        public UsedGamesAPIGameResponse(bool success)
        {
            Success = success;
        }
    }
}
