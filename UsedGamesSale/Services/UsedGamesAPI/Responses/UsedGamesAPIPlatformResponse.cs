using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.UsedGamesAPI.Responses
{
    public class UsedGamesAPIPlatformResponse 
    {
        public List<Platform> Platforms { get; set; }
        public bool Success { get; set; }
        public bool IsUnauthorized { get; set; }
    }
}
