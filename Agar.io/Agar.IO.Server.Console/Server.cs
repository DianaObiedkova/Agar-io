using Agar.IO.Server.Console.Controllers;
using Agar.IO.Server.Console.Models;
using Agar.IO.Server.Console.Models.Commands;
using System;

namespace Agar.IO.Server.Console
{
    internal class Server
    {
        public const int maxFoodAmount = 100;
        public const int fieldWidth = 3000;
        public const int fieldHeight = 3000;

        public readonly static Random random = new Random();
        public GameState Game { get; set; }
        public ConnectionController ConnectionController { get; set; }
        public Server(ConnectionController connectionManager)
        {
            ConnectionController = connectionManager;
        }
        private GameState NewGame()
        {
            var game = new GameState();
            for (int i = 0; i < maxFoodAmount; i++)
            {
                game.FoodList.Add(new Food(random.Next(fieldWidth), random.Next(fieldHeight), random.Next(5, 10)));
            }

            return game;
        }

        public void Start()
        {
            Game = NewGame();
            ConnectionController.PlayerCommandAction = ExecuteClientCommand;
            ConnectionController.NewPlayerAction = AddPlayer;
        }

        private void AddPlayer(string name)
        {
            var newPlayer = new Player(name);

            Game.GameStateLock.EnterWriteLock(); //пока состояние сериализуется, не должно быть никаких изменений с ним
            Game.Players.Add(newPlayer);
            ConnectionController.SendToAllClients(new UpdateState(Game));
            Game.GameStateLock.ExitWriteLock();
        }

        private void ExecuteClientCommand(string arg1, BaseCommand arg2)
        {
            Game.GameStateLock.EnterReadLock();
            arg2.Execute(this, arg1);
            Game.GameStateLock.ExitReadLock();

            Game.GameStateLock.EnterWriteLock();
            ConnectionController.SendToAllClients(new UpdateState(Game));
            Game.GameStateLock.ExitWriteLock();
        }

        internal void RemovePlayer(string playerName, string message)
        {
            ConnectionController.SendToClient(playerName, new End(message));
            Game.Players.RemoveAll(x => x.Name.Equals(playerName));
            ConnectionController.EndConnection(playerName);
        }
    }
}
