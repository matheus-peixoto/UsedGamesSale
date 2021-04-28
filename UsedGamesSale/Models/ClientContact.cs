namespace UsedGamesSale.Models
{
    public class ClientContact
    {
        public int Id { get; set; }

        public string PhoneNumber { get; set; }
        public Client Client { get; set; }
    }
}
