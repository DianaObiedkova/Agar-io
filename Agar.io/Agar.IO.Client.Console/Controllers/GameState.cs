using Agar.IO.Client.WinForms.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Controllers
{
    [ProtoContract]
    public class GameState
    {
        [ProtoMember(1)]
        public List<Player> Players { get; set; }
        [ProtoMember(2)]
        public List<Food> FoodList { get; set; }
        [ProtoIgnore]
        public Player CurrentPlayer { get; set; }
        [ProtoIgnore]
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
