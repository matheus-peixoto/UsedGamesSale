using UsedGamesSale.Models.Enums;

namespace UsedGamesSale.Models
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        public int Quantity { get; set; }
        public Game Game { get; set; }
        public Client Client { get; set; }
    }
}
