using Agar.IO.Client.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Controllers
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public List<Food> FoodList { get; set; }
        public Player CurrentPlayer { get; set; }
        public List<Food> EatenFoodPrediction { get; set; }
        public GameState()
        {
            Players = new List<Player>();
            FoodList = new List<Food>();
        }

        public GameState DeepClonePrediction()
        {
            var copy = (GameState)MemberwiseClone();

            copy.FoodList = FoodList;
            copy.CurrentPlayer = CurrentPlayer;

            return copy;
        }
    }
}
