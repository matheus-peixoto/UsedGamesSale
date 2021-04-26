using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIClients : UsedGamesAPI
    {
        public UsedGamesAPIClients(IHttpClientFactory clientFactory) : base(clientFactory, "clients/") { }
    }
}
