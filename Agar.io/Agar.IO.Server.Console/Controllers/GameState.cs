using Agar.IO.Server.Console.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Agar.IO.Server.Console
{
    public class GameState
    {
        public List<Player> Players { get; set; }
        public List<Food> FoodList { get; set; }
        public ReaderWriterLockSlim GameStateLock { get; set; }
        public GameState()
        {
            Players = new List<Player>();
            FoodList = new List<Food>();
            GameStateLock = new ReaderWriterLockSlim();
        }
    }
}
