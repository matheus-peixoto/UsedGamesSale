using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUsedGamesAPI
    {
        public void ConfigureToken(string value);

        public Task<UsedGamesAPIResponse> LoginAsync(UserLoginDTO userDTO);
    }
}
