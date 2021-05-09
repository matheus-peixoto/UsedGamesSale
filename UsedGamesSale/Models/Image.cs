namespace UsedGamesSale.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }

        public Image() { }

        public Image(string path)
        {
            Path = path;
        }

        public Image(string path, int gameId)
        {
            Path = path;
            GameId = gameId;
        }

        public Image(int id, string path, int gameId)
        {
            Id = id;
            Path = path;
            GameId = gameId;
        }
    }
}
