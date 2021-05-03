using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsedGamesSale.Models.ViewModels
{
    public class RegisterGameViewModel
    {
        public Game Game { get; set; }
        public SelectList Platforms { get; set; }
        public int ImgsPerGame { get; set; }
        public string[] TempImgsPaths { get; set; }

        public RegisterGameViewModel() { }

        public RegisterGameViewModel(SelectList platforms, int imgsPerGame)
        {
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
        }

        public RegisterGameViewModel(Game game, SelectList platforms, int imgsPerGame, string[] tempImgsPaths)
        {
            Game = game;
            Platforms = platforms;
            ImgsPerGame = imgsPerGame;
            TempImgsPaths = tempImgsPaths;
        }
    }
}