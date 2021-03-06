using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPISellerResponse : UsedGamesAPIResponse
    {
        public List<Game> Games { get; set; }
        public bool IsUnauthorized { get; set; }
    }
}
