using Agar.IO.Server.Models.Communication.Classes;
using Agar.IO.Server.Models.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Game
    {
        /// <summary>
        /// We've used singleton pattern here 
        /// Sixth version of article (https://csharpindepth.com/articles/singleton)
        /// </summary>
        private static readonly Lazy<Game> lazy = new Lazy<Game>(() => new Game());
        public static Game Instance { get { return lazy.Value; } }

        private readonly FoodFactory foodFactory;
        readonly int MaxFoodCount = 10000;
        readonly Random ran = new Random();
        int botCounter = 0;

        //public List<Player> players;
        public List<Food> foodList;

        private Dictionary<ICommunicator, Player> players;
        private Dictionary<ICommunicator, DateTime> lastUpdate;
        public int FieldWidth { get; }
        public int FieldHeight { get; }

        public Game()
        {
            // players = new List<Player>();
            players = new Dictionary<ICommunicator, Player>();
            lastUpdate = new Dictionary<ICommunicator, DateTime>();
            foodList = new List<Food>();
            FieldHeight = FieldWidth = 1000000;
            foodFactory = new FoodFactory(FieldWidth, FieldHeight);
            for(int i = 0; i < MaxFoodCount; i++)
            {
                foodList.Add(foodFactory.Create());
            }
        }

        private string GetRandomColor()
        {
            List<string> colors=new List<string>();
            Type colorType = typeof(Color);
            // We take only static property to avoid properties like Name, IsSystemColor ...
            PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
            foreach (PropertyInfo propInfo in propInfos)
            {
                colors.Add(propInfo.Name);
            }

            return colors[ran.Next(colors.Count)];
        }

        private Position GetFreePosition()
        {
            int x = ran.Next(FieldWidth);
            int y = ran.Next(FieldHeight);

            bool free = true;

            foreach (var (id, value) in players.Tuples())
            {
                if ((x > value.Location.X - value.Radius && x < value.Location.X + value.Radius) //or 2*Radius????
                    || (y > value.Location.Y - value.Radius && y < value.Location.Y + value.Radius))
                {
                    free = false;
                }
            }

            if (free) return new Position(x, y);
            return GetFreePosition();
        }

        public Player AddNewPlayer(string userName, ICommunicator communicator)
        {
            var newPlayer = new Player(Guid.NewGuid().ToString(), "Player " + userName)
            {
                Score = 0,
                Weight = 5,
                Color = GetRandomColor(),
                Location = GetFreePosition()
            };

            players.Add(communicator, newPlayer);
            lastUpdate.Add(communicator, DateTime.Now);
            communicator.Send(new Message(IO.Server.Models.Communication.Enums.EventType.SpawnMyself, newPlayer));
            TransferToAll(new Message(IO.Server.Models.Communication.Enums.EventType.Spawn, newPlayer),newPlayer);

            return newPlayer;
        }

        public Player AddNewBotPlayer(ICommunicator communicator)
        {
            var newBot = new BotPlayer(Guid.NewGuid().ToString(), "Bot " + (++botCounter).ToString())
            {
                Score = 0,
                Weight = 5,
                Color = GetRandomColor(),
                Location = GetFreePosition()
            };
            players.Add(communicator,newBot);
            lastUpdate.Add(communicator, DateTime.UtcNow);
            communicator.Send(new Message(IO.Server.Models.Communication.Enums.EventType.SpawnMyself, newBot));
            TransferToAll(new Message(IO.Server.Models.Communication.Enums.EventType.Spawn, newBot), newBot);

            return newBot;
        }

        public bool CheckIntersection(Player player)
        {
            bool dead = false;

            foreach (var (com, pl) in players.Tuples())
            {
                if (player.IsIntersecting(pl))
                {
                    Player killed, extended;

                    if (player.Weight > pl.Weight)
                    {
                        killed = pl;
                        extended = player;
                    }
                    else if(player.Weight < pl.Weight)
                    {
                        killed = player;
                        extended = pl;
                        dead = true;
                    }
                    else { return false; }

                    extended.Eat(killed);
                    Kill(players.FirstOrDefault(x => x.Value == killed).Key);
                    TransferToAll(new Message(IO.Server.Models.Communication.Enums.EventType.SizeChange, extended));

                }
            }

            return dead;
        }

        public void Move(ICommunicator com, Position pos)
        {
            players.TryGetValue(com, out Player player);
            lastUpdate.TryGetValue(com, out DateTime time);
            if (!lastUpdate.ContainsKey(com)) return;

            lastUpdate.Add(com, DateTime.UtcNow);

            player.Location = pos;
            if (CheckIntersection(player)) return;

            TransferToAll(new Message(IO.Server.Models.Communication.Enums.EventType.CoordsChange, player));
        }

        public void Kill(ICommunicator com)
        {
            players.TryGetValue(com, out Player player);
            TransferToAll(new Message(IO.Server.Models.Communication.Enums.EventType.Die, player));
            players.Remove(com);
            lastUpdate.Remove(com);
        }

        private void TransferToAll(Message message)
        {
            foreach (var (id, value) in players.Tuples())
            {
                id.Send(message);
            }
        }

        private void TransferToAll(Message message, Player playerToExclude)
        {
            foreach (var (id, value) in players.Tuples())
            {
                if(!value.Equals(playerToExclude))
                    id.Send(message);
            }
        }
        //public void EatOrRemovePlayer(Player player)
        //{
        //    players.Remove(player);
        //}
    }

    public static class IDictionaryExtensions
    {
        public static IEnumerable<(TKey, TValue)> Tuples<TKey, TValue>(
            this IDictionary<TKey, TValue> dict)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
                yield return (kvp.Key, kvp.Value);
        }
    }
}
