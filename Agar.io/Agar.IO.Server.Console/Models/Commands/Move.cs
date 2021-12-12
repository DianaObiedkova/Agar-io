using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    class Move : BaseCommand
    {
        [ProtoMember(1)]
        public List<Tuple<int, float, float, float>> Movement { get; set; }
        [ProtoMember(2)]
        public long Time { get; set; }
        
        public override async void Execute(Server server, string name)
        {
            bool invalid = false;
            var game = server.Game;
            Player player = game.Players.Find(x => x.Name.Equals(name));
            if (player == null) return;

            if (Time < player.LastMovementTime)
                return;

            if (player.FirstMovementServerTime == 0)
            {
                player.FirstMovementServerTime = Stopwatch.GetTimestamp();
                player.LastMovementTime = Time;
                player.FirstMovementTime = Time;
            }


            if (ExecuteEatingFood(player, server.Game)) invalid = true;
            if (ExecuteEatingPlayer(player, server)) invalid = true;

            if (invalid) await server.ConnectionController.SendToClient(name, new Invalid("Invalid movement"));
        }

        private bool ExecuteEatingPlayer(Player player, Server server)
        {
            var invalid = false;
            var eatenPlayers = new List<Player>();

            foreach(var other in server.Game.Players)
            {

                if (player.Name == other.Name)
                {
                    continue;
                }

                if (CanBeEaten(player, other)){
                    player.Weight += other.Weight;
                    eatenPlayers.Add(other);
                }
                else if (CanBeEaten(other, player))
                {
                    other.Weight += player.Weight;
                    eatenPlayers.Add(player);
                }
            }

            foreach (var eatenPlayer in eatenPlayers)
            {
                server.RemovePlayer(eatenPlayer.Name, "You have been eaten!");
            }

            return invalid;
        }

        private bool ExecuteEatingFood(Player player, GameState game)
        {
            var invalid = false;
            var eatenFood = new List<Food>();
            var newFood = new List<Food>();
            
            foreach(var item in game.FoodList)
            {
                if (CanBeEaten(item, player))
                {
                    //check validation

                    player.Weight += item.Weight;
                    eatenFood.Add(item);
                    newFood.Add(CreateFood());
                }
            }

            game.FoodList.RemoveAll(x => eatenFood.Contains(x));
            newFood.ForEach(x => game.FoodList.Add(x));
            return invalid;
        }

        private Food CreateFood()
        {
            return new Food(Server.random.Next(Server.fieldWidth),
                        Server.random.Next(Server.fieldHeight),
                        Server.random.Next(5, 15)
                );
        }

        private bool CanBeEaten(Food food, Player player)
        {
            var offsetX = food.X - player.X;
            var offsetY = food.Y - player.Y;
            var distance = Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2));

            if(distance<player.Radius - food.Radius && player.Weight> 1.25 * food.Weight) return true;
            return false;
        }

        private bool CanBeEaten(Player eating, Player toBeEaten)
        {
            var offsetX = eating.X - toBeEaten.X;
            var offsetY = eating.Y - toBeEaten.Y;
            var distance = Math.Sqrt(Math.Pow(offsetX, 2) + Math.Pow(offsetY, 2));

            if (distance < toBeEaten.Radius - eating.Radius && toBeEaten.Weight > 1.25 * toBeEaten.Weight) return true;
            return false;
        }
    }
}
