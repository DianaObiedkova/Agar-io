using Agar.IO.Server.Console;
using Agar.IO.Server.Console.Controllers;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var ConnectionController = new ConnectionController();
            Console.WriteLine("Connection started...");
            var task = ConnectionController.StartListeningAsync();

            Console.WriteLine("New game!");
            var game = new Server(ConnectionController);
            game.Start();
            Console.WriteLine("Server started...");

            Console.WriteLine("Waiting for the connection controller task to complete.");
            if (!task.IsCompleted)
                task.Wait();
        }
    }
}
