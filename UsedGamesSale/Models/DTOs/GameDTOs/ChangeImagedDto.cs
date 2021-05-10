using Microsoft.AspNetCore.Http;

namespace UsedGamesSale.Models.DTOs.GameDTOs
{
    public class ChangeImagedDto
    {
        public int GameId { get; set; }
        public string[] TempImgsPaths { get; set; }
        public int ImgId { get; set; }

        public IFormFile ImgFile { get; set; }
        public string OldImgRelativePath { get; set; }

        public ChangeImagedDto() { }

    }
}
