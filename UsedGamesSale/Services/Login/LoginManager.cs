using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UsedGamesSale.Models;
using UsedGamesSale.Services.Login.Interfaces;

namespace UsedGamesSale.Services.Login
{
    public abstract class LoginManager : ILoginManager
    {
        protected readonly IHttpContextAccessor _httpContext;
        protected readonly string _key;
        protected readonly string _tokenKey;

        public LoginManager(IHttpContextAccessor httpContext, string key)
        {
            _httpContext = httpContext;
            _key = key;
            _tokenKey = key + ".Token";
        }

        public User GetUser()
        {
            string jsonModel = _httpContext.HttpContext.Session.GetString(_key);
            if (string.IsNullOrEmpty(jsonModel))
                return null;

            return JsonConvert.DeserializeObject<User>(jsonModel);
        }

        public int GetUserId()
        {
            string jsonModel = _httpContext.HttpContext.Session.GetString(_key);
            return JsonConvert.DeserializeObject<User>(jsonModel).Id;
        }

        public string GetUserToken() => _httpContext.HttpContext.Session.GetString(_tokenKey);

        public void Login(User user, string token)
        {
            string jsonModel = JsonConvert.SerializeObject(user);
            _httpContext.HttpContext.Session.SetString(_key, jsonModel);
            _httpContext.HttpContext.Session.SetString(_tokenKey, token);
        }

        public void Logout()
        {
            _httpContext.HttpContext.Session.Remove(_key);
            _httpContext.HttpContext.Session.Remove(_tokenKey);
        }

        public bool IsLogged() => !string.IsNullOrEmpty(_httpContext.HttpContext.Session.GetString(_key));
    }
}
