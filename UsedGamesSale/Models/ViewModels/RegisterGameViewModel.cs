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
    }
}
