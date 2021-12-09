using Agar.IO.Server.Console.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console
{
    class Game
    {
        public List<Player> Players { get; set; }
        public List<Food> FoodList { get; set; }
        public Game()
        {
            Players = new List<Player>();
            FoodList = new List<Food>();
        }
    }
}
