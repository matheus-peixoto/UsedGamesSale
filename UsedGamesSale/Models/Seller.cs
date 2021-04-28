using System.Collections.Generic;

namespace UsedGamesSale.Models
{
    public class Seller : User
    {
        public List<Game> Games { get; set; }
        public SellerContact SellerContact { get; set; }
    }
}
