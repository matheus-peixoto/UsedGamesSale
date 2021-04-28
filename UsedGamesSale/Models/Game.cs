using System;
using System.Collections.Generic;

namespace UsedGamesSale.Models
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public string Details { get; set; }
        public int StockQuantity { get; set; }
        public List<Order> Order { get; set; }
        public Platform Platform { get; set; }
    }
}