using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + "games/");
        }

        public async Task<UsedGamesAPIGameResponse> Get(int id, string token)
        {
            ConfigureToken(token);
            HttpResponseMessage responseMsg = await _client.GetAsync(_client.BaseAddress.AbsoluteUri + id);
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse() { Success = responseMsg.IsSuccessStatusCode };
            if (response.Success) response.Game = JsonConvert.DeserializeObject<Game>(responseStr);
            return response;
        }

            public async Task<UsedGamesAPIGameResponse> CreateAsync(Game game, string token)
        {
            ConfigureToken(token);
            string jsonGame = JsonConvert.SerializeObject(game);
            HttpResponseMessage responseMsg = await _client.PostAsync(_client.BaseAddress, new StringContent(jsonGame, Encoding.UTF8, "application/json"));
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse() { Success = responseMsg.IsSuccessStatusCode };
            if (response.Success) response.Game = JsonConvert.DeserializeObject<Game>(responseStr);

            return response;
        }

        private void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }

    }
}
