using Agar.IO.Client.WinForms.Models;
using Agar.IO.Client.WinForms.Models.Commands;
using System;
using System.Collections.Generic;

namespace Agar.IO.Client.WinForms.Handlers
{
    class MovementHandler : Handler
    {
        public MovementHandler(Position mousePosition) : base(mousePosition)
        {
        }
        public override void Execute(Game game)
        {
            var state = game.GameState;

            if (state?.CurrentPlayer == null)
                return;

            var command = new Move
            {
                Time = game.Time,
                Movement = new List<Tuple< double, double, double>>()
            };

            var player = state.CurrentPlayer;

            double vX = X - state.CurrentPlayer.X;
            double vY = Y - state.CurrentPlayer.Y;

            double size = (float)(Math.Sqrt(vX * vX + vY * vY));
            if (size == 0)
                return;
            vX /= size;
            vY /= size;

            var nextX = state.CurrentPlayer.X + vX * state.CurrentPlayer.Speed;
            var nextY = state.CurrentPlayer.Y + vY * state.CurrentPlayer.Speed;

            if (nextX > Game.FieldWidth)
                nextX = Game.FieldWidth;
            if (nextX < 0)
                nextX = 0;
            if (nextY > Game.FieldHeight)
                nextY = Game.FieldHeight;
            if (nextY < 0)
                nextY = 0;

            var eatenFood = new List<Food>();

            foreach (var food in state.FoodList)
                if (CanBeEaten(food, nextX, nextY, player))
                {
                    player.Weight += food.Weight;
                    eatenFood.Add(food);
                    if (!state.EatenFoodPrediction.Contains(food))
                        state.EatenFoodPrediction.Add(food);
                }
            game.GameState.FoodList.RemoveAll(f => eatenFood.Contains(f));

            command.Movement.Add(new Tuple< double, double, double>(nextX, nextY, player.Weight));
            

            foreach (var move in command.Movement)
            {
                var part = game.GameState.CurrentPlayer;
                part.X = move.Item1;
                part.Y = move.Item2;
            }

            game.ServerConnection.SendAsync(command);
        }

        private static bool CanBeEaten(Food food, double nextX, double nextY, Player player)
        {
            var dx = food.X - nextX;
            var dy = food.Y - nextY;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            return (distance < player.Radius - food.Radius &&    
                player.Weight > 1.25 * player.Weight);
        }
    }
}
