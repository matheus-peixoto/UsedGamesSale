using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPISellers : UsedGamesAPI
    {
        public UsedGamesAPISellers(IHttpClientFactory clientFactory) : base(clientFactory, "sellers/") { }
    }
}
