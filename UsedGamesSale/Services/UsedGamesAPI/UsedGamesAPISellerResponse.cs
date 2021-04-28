using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPISellerResponse
    {
        public List<Game> Games { get; set; }
        public bool Success { get; set; }
        public bool IsUnauthorized { get; set; }
    }
}
