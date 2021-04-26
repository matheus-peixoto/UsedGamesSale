using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using UsedGamesSale.Services.Login.Interfaces;

namespace UsedGamesSale.Services.Login
{
    public abstract class LoginManager<T> : ILoginManager<T>
        where T : class
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

        public T GetUser()
        {
            string jsonModel = _httpContext.HttpContext.Session.GetString(_key);
            if (string.IsNullOrEmpty(jsonModel))
                return null;

            return JsonConvert.DeserializeObject<T>(jsonModel);
        }

        public string GetUserToken() => _httpContext.HttpContext.Session.GetString(_tokenKey);

        public void Login(T model, string token)
        {
            string jsonModel = JsonConvert.SerializeObject(model);
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
