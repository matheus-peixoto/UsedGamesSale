using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUsedGamesAPIPlatforms
    {
        public Task<UsedGamesAPIPlatformResponse> GetPlatformsAsync();
    }
}
