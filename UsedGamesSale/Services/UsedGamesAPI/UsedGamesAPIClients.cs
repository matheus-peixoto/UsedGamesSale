using System.Net.Http;

namespace UsedGamesSale.Services.UsedGamesAPI
{
    public class UsedGamesAPIClients : UsedGamesAPI
    {
        public UsedGamesAPIClients(IHttpClientFactory clientFactory) : base(clientFactory, "clients/") { }
    }
}
