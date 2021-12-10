using Agar.IO.Server.Console.Controllers;
using Agar.IO.Server.Console.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console
{
    class Server
    {
        public const int maxFoodAmount = 100;
        public const int fieldWidth = 3000;
        public const int fieldHeight = 3000;

        public readonly static Random random = new Random();
        public Game Game { get; set; }
        public ConnectionController ConnectionController { get; set; }

        private Game NewGame()
        {
            var game = new Game();
            for (int i = 0; i < maxFoodAmount; i++)
            {
                game.FoodList.Add(new Food(random.Next(fieldWidth), random.Next(fieldHeight), random.Next(5, 10)));
            }

            return game;
        }

        public void Start()
        {
            Game = NewGame();
        }
    }
}
