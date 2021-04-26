﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models;
using UsedGamesSale.Models.DTOs.User;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIClients
    {
        private HttpClient _client;

        public UsedGamesAPIClients(IHttpClientFactory clientFactory)
        {
            _client = clientFactory.CreateClient("UsedGamesAPI");
            _client.BaseAddress = new Uri(_client.BaseAddress.AbsoluteUri + "clients/");
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
            if (responseMsg.StatusCode == HttpStatusCode.Unauthorized) { }
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIResponse response = JsonConvert.DeserializeObject<UsedGamesAPIResponse>(responseStr);
            response.Success = responseMsg.IsSuccessStatusCode;
            return response;
        }
    }
}
