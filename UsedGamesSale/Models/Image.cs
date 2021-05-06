namespace UsedGamesSale.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public Game Game { get; set; }

        public Image() { }

        public Image(string path)
        {
            Path = path;
        }
    }
}
