using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Models;

namespace UsedGamesSale.Services.Login
{
    public class SellerLoginManager : LoginManager
    {
        public SellerLoginManager(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, "Login.Seller") { }
    }
}
