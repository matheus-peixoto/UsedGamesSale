using UsedGamesSale.Models;

namespace UsedGamesSale.Services.Login.Interfaces
{
    public interface ILoginManager
    {
        User GetUser();
        int GetUserId();
        string GetUserToken();
        void Login(User user, string token);
        void Logout();
        bool IsLogged();
    }
}
