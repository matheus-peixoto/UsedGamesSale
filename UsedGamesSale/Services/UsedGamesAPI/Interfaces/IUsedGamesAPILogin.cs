using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI.Interfaces
{
    public interface IUsedGamesAPILogin
    {
        public Task<UsedGamesAPILoginResponse> LoginAsync(UserLoginDTO userDTO);
    }
}
