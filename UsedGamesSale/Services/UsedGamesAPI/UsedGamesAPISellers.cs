using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;
using UsedGamesSale.Services.UsedGamesAPI.Interfaces;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPISellers : UsedGamesAPI, IUsedGamesAPISellers
    {
        public UsedGamesAPISellers(IHttpClientFactory clientFactory) : base(clientFactory, "sellers/") { }

        public async Task<UsedGamesAPISellerResponse> GetGamesAsync(int sellerId, string token)
        {
            ConfigureToken(token);
            HttpResponseMessage responseMsg = await _client.GetAsync(_client.BaseAddress + $"{sellerId}/games");
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPISellerResponse response = JsonConvert.DeserializeObject<UsedGamesAPISellerResponse>(responseStr);
            response.Success = responseMsg.IsSuccessStatusCode;
            if (responseMsg.StatusCode == HttpStatusCode.Unauthorized) response.IsUnauthorized = true;

            return response;
        }
    }
}
