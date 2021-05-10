using System.Net.Http;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIClients : UsedGamesAPILogin
    {
        public UsedGamesAPIClients(IHttpClientFactory clientFactory) : base(clientFactory, "clients") { }
    }
}
