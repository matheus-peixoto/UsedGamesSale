using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public abstract class UsedGamesAPI : IUsedGamesAPI
    {
        private readonly HttpClient _client;

        public UsedGamesAPI(IHttpClientFactory clientFactory, string endpoint)
        {
            _client = clientFactory.CreateClient("UsedGamesAPI");
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + endpoint);
        }

        public void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }

        public async Task<UsedGamesAPIResponse> LoginAsync(UserLoginDTO userDTO)
        {
            string jsonUser = JsonConvert.SerializeObject(userDTO);
            HttpResponseMessage responseMsg = await _client.PostAsync(_client.BaseAddress + "login", new StringContent(jsonUser, Encoding.UTF8, "application/json"));
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIResponse response = JsonConvert.DeserializeObject<UsedGamesAPIResponse>(responseStr);
            response.Success = responseMsg.IsSuccessStatusCode;
            return response;
        }
    }
}
