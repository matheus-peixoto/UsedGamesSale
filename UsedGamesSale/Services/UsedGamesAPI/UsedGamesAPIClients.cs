using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UsedGamesSale.Models;

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
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }

        public async Task Login(User user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);
            HttpResponseMessage response = await _client.PostAsync(_client.BaseAddress + "login", new StringContent(jsonUser, Encoding.UTF8, "application/json"));
        }
    }
}
