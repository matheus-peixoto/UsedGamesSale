using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UsedGamesSale.Services.Filters;
using UsedGamesSale.Services.Image;
using UsedGamesSale.Services.Login;
using UsedGamesSale.Services.UsedGamesAPI;

namespace UsedGamesSale.Areas.Seller.Controllers
{
    [Area("Seller")]
    [ValidateSellerLogin]
    public class GameController : Controller
    {
        private readonly UsedGamesAPISellers _usedGamesAPISellers;
        private readonly int _imgsPerGame;
        private readonly IConfiguration _configuration;
        private SellerLoginManager _sellerLoginManager;

        public GameController(UsedGamesAPISellers usedGamesAPISellers, IConfiguration configuration, SellerLoginManager sellerLoginManager)
        {
            _usedGamesAPISellers = usedGamesAPISellers;
            _configuration = configuration;
            _imgsPerGame = configuration.GetValue<int>("Game:ImgsPerGame");
            _sellerLoginManager = sellerLoginManager;
        }
    }
}
