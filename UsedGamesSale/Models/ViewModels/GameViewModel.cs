using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UsedGamesSale.Models.ViewModels
{
    public class GameViewModel
    {
        public Game Game { get; set; }
        public SelectList Platforms { get; set; }
        public int ImgsPerGame { get; set; }
        public int SellerId { get; set; }
        public string[] TempImgsPaths { get; set; }
        public GameViewModel() { }

        public GameViewModel(SelectList platforms, int imgsPerGame, int sellerId)
        {
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
            SellerId = sellerId;
        }

        public GameViewModel(Game game, SelectList platforms, int imgsPerGame,  int sellerId) : this (platforms, imgsPerGame, sellerId)
        {
            Game = game;
        }

        public GameViewModel(Game game, SelectList platforms, int imgsPerGame, int sellerId, string[] tempImgsPaths) : this(game, platforms, imgsPerGame, sellerId)
        {
            TempImgsPaths = tempImgsPaths;
        }
    }
}