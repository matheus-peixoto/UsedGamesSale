namespace UsedGamesSale.Models
{
    public class SellerContact
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public Seller Seller { get; set; }
    }
}
