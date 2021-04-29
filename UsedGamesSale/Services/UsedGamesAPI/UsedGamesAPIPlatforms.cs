using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;
using UsedGamesSale.Services.UsedGamesAPI.Responses;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIPlatforms : IUsedGamesAPIPlatforms
    {
        private readonly HttpClient _client;

        public UsedGamesAPIPlatforms(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UsedGamesAPI");
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + "platforms");
        }

        public async Task<UsedGamesAPIPlatformResponse> GetPlatformsAsync()
        {
            HttpResponseMessage responseMsg = await _client.GetAsync(_client.BaseAddress);
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIPlatformResponse response = JsonConvert.DeserializeObject<UsedGamesAPIPlatformResponse>(responseStr);
            response.Success = responseMsg.IsSuccessStatusCode;
            if (responseMsg.StatusCode == HttpStatusCode.Unauthorized) response.IsUnauthorized = true;

            return response;
        }

        protected void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }
    }
}
