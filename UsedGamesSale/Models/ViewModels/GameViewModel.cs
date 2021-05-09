using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UsedGamesSale.Models.ViewModels
{
    public class GameViewModel
    {
        public Game Game { get; set; }
        public SelectList Platforms { get; set; }
        public int ImgsPerGame { get; set; }
        public string[] TempImgsPaths { get; set; }
        public int SellerId { get; set; }
        public int GameId { get; set; }
        public int ImgId { get; set; }

        public IFormFile ImgFile { get; set; }
        public string OldImgRelativePath { get; set; }

        public GameViewModel() { }

        public GameViewModel(SelectList platforms, int imgsPerGame, int sellerId)
        {
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
            SellerId = sellerId;
        }

        public GameViewModel(Game game, SelectList platforms, int imgsPerGame,  int sellerId)
        {
            Game = game;
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
            SellerId = sellerId;
        }

        public GameViewModel(Game game, SelectList platforms, int imgsPerGame, string[] tempImgsPaths, int sellerId)
        {
            Game = game;
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
            TempImgsPaths = tempImgsPaths;
            SellerId = sellerId;
        }
    }
}