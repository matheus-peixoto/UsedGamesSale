using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.Login
{
    public class ClientLoginManager : LoginManager
    {
        public ClientLoginManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "Login.Client")  { }
    }
}
