using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIGames : IUSedGamesAPIGames
    {
        private readonly HttpClient _client;

        public UsedGamesAPIGames(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UsedGamesAPI");
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + "games");
        }

        public async Task<UsedGamesAPIGameResponse> CreateGameAsync(Game game, string token)
        {
            ConfigureToken(token);
            string jsonGame = JsonConvert.SerializeObject(game);
            HttpResponseMessage responseMsg = await _client.PostAsync(_client.BaseAddress, new StringContent(jsonGame, Encoding.UTF8, "application/json"));
            UsedGamesAPIResponse response = new UsedGamesAPIResponse() { Success = responseMsg.IsSuccessStatusCode };
            return (UsedGamesAPIGameResponse)response;
        }

        private void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }

    }
}
