using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public abstract class UsedGamesAPILogin : IUsedGamesAPILogin
    {
        protected readonly HttpClient _client;

        public UsedGamesAPILogin(IHttpClientFactory clientFactory, string endpoint)
        {
            _client = clientFactory.CreateClient("UsedGamesAPI");
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + $"{endpoint}/");
        }

        public async Task<UsedGamesAPILoginResponse> LoginAsync(UserLoginDTO userDTO)
        {
            string jsonUser = JsonConvert.SerializeObject(userDTO);
            HttpResponseMessage responseMsg = await _client.PostAsync(_client.BaseAddress + "login", new StringContent(jsonUser, Encoding.UTF8, "application/json"));
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPILoginResponse response = JsonConvert.DeserializeObject<UsedGamesAPILoginResponse>(responseStr);
            response.Success = responseMsg.IsSuccessStatusCode;
            return response;
        }

        protected void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }
    }
}
