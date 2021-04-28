using System.Collections.Generic;

namespace UsedGamesSale.Models
{
    public class Client : User
    {
        public List<Order> Orders { get; set; }
        public ClientContact ClientContact { get; set; }
    }
}
