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

        public async Task<UsedGamesAPIGameResponse> GetAsync(int id, string token)
        {
            ConfigureToken(token);
            HttpResponseMessage responseMsg = await _client.GetAsync(_client.BaseAddress.AbsoluteUri + id);
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse() { Success = responseMsg.IsSuccessStatusCode };
            if (response.Success) response.Game = JsonConvert.DeserializeObject<Game>(responseStr);
            return response;
        }

        public async Task<UsedGamesAPIGameResponse> GetImagesAsync(int id, string token)
        {
            ConfigureToken(token);
            HttpResponseMessage responseMsg = await _client.GetAsync($"{_client.BaseAddress}{id}/images");
            string responseStr = await responseMsg.Content.ReadAsStringAsync();
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse(responseMsg.IsSuccessStatusCode);
            if (response.Success) 
            { 
                response.Images = JsonConvert.DeserializeObject<List<Image>>(responseStr); 
            }
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

        public async Task<UsedGamesAPIGameResponse> CreateImagesAsync(int id, List<string> imgPaths, string token)
        {
            ConfigureToken(token);
            var imgs = new { Images = BuildImages(imgPaths) };
            string jsonImgs = JsonConvert.SerializeObject(imgs);
            HttpResponseMessage responseMsg = await _client.PostAsync(_client.BaseAddress + $"{id}/images/", new StringContent(jsonImgs, Encoding.UTF8, "application/json"));
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse(success: responseMsg.IsSuccessStatusCode);
            return response;
        }

        public async Task<UsedGamesAPIGameResponse> UpdateAsync(Game game, string token)
        {
            ConfigureToken(token);
            string jsonGame = JsonConvert.SerializeObject(game);
            HttpResponseMessage responseMsg = await _client.PutAsync($"{_client.BaseAddress}{game.Id}", new StringContent(jsonGame, Encoding.UTF8, "application/json"));
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse(responseMsg.IsSuccessStatusCode);
            return response;
        }

        public async Task<UsedGamesAPIGameResponse> UpdateImageAsync(Image img, string token)
        {
            ConfigureToken(token);
            string requestUri = $"{_client.BaseAddress}{img.GameId}/images/{img.Id}";
            string jsonImg = JsonConvert.SerializeObject(img);
            HttpResponseMessage responseMsg = await _client.PutAsync(requestUri, new StringContent(jsonImg, Encoding.UTF8, "application/json"));
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse(success: responseMsg.IsSuccessStatusCode);
            return response;
        }

        public async Task<UsedGamesAPIGameResponse> DeleteAsync(int id, string token)
        {
            ConfigureToken(token);
            HttpResponseMessage responseMsg = await _client.DeleteAsync(_client.BaseAddress + id.ToString());
            UsedGamesAPIGameResponse response = new UsedGamesAPIGameResponse(responseMsg.IsSuccessStatusCode);
            return response;
        }

        private List<Image> BuildImages(List<string> imgPaths)
        {
            List<Image> imgs = new List<Image>();
            foreach (string imgPath in imgPaths)
            {
                imgs.Add(new Image(imgPath));
            }
            return imgs;
        }

        private void ConfigureToken(string value)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {value}");
        }
    }
}
