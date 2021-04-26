namespace UsedGamesSale.Services.Login.Interfaces
{
    public interface ILoginManager<T> where T : class
    {
        T GetUser();
        string GetUserToken();
        void Login(T model, string token);
        void Logout();
        bool IsLogged();
    }
}
