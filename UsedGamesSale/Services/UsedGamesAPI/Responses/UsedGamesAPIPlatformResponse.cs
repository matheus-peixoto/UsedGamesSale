using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPIPlatformResponse : UsedGamesAPIResponse
    {
        public List<Platform> Platforms { get; set; }
        public bool IsUnauthorized { get; set; }
    }
}
