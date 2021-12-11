using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    class Move : BaseCommand
    {
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


            if (CouldEatFood(player, server.Game)) invalid = true;
            if (CouldEatPlayer(player, server)) invalid = true;

            if (invalid) await server.ConnectionController.SendToClient(name, new Invalid("Invalid movement"));
        }

        private bool CouldEatPlayer(Player player, Server server)
        {
            throw new NotImplementedException();
        }

        private bool CouldEatFood(Player player, Game game)
        {
            throw new NotImplementedException();
        }
    }
}
