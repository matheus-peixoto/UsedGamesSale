using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedGamesSale.Models.ViewModels
{
    public class GameViewModel
    {
        public Game Game { get; set; }
        public SelectList Platforms { get; set; }
        public int ImgsPerGame { get; set; }
        public string[] TempImgsPaths { get; set; }
        public int SellerId { get; set; }

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