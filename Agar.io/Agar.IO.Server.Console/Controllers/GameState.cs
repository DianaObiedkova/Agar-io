using Agar.IO.Server.Console.Models;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Agar.IO.Server.Console
{
    [ProtoContract]
    public class GameState
    {
        [ProtoMember(1)]
        public List<Player> Players { get; set; }
        [ProtoMember(2)]
        public List<Food> FoodList { get; set; }
        [ProtoIgnore]
        public ReaderWriterLockSlim GameStateLock { get; set; }
        public GameState()
        {
            Players = new List<Player>();
            FoodList = new List<Food>();
            GameStateLock = new ReaderWriterLockSlim();
        }
    }
}
