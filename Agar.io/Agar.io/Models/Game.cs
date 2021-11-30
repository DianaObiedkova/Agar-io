using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Game
    {
        private readonly FoodFactory foodFactory;
        readonly int MaxFoodCount = 10000;
        readonly Random ran = new Random();
        int botCounter = 0;

        public List<Player> players;
        public List<Food> foodList;
        public int FieldWidth { get; }
        public int FieldHeight { get; }

        public Game()
        {
            players = new List<Player>();
            foodList = new List<Food>();
            FieldHeight = FieldWidth = 1000000;
            foodFactory = new FoodFactory(FieldWidth, FieldHeight);
            for(int i = 0; i < MaxFoodCount; i++)
            {
                foodList.Add(foodFactory.Create());
            }
        }

        public void AddNewPlayer(string connectionId)
        {
            players.Add(new Player(connectionId, "Player " + connectionId)
            {
                Location = new Position(ran.Next(FieldWidth - 1), ran.Next(FieldHeight - 1))
            });

        }

        public void AddNewBotPlayer()
        {
            players.Add(new BotPlayer(Guid.NewGuid().ToString(), "Bot " + (++botCounter).ToString())
            {
                Location = new Position(ran.Next(FieldWidth - 1), ran.Next(FieldHeight - 1))
            });
        }

        public void EatOrRemovePlayer(Player player)
        {
            players.Remove(player);
        }
    }
}
