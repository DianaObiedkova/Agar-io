using Agar.IO.Server.Console.Models.Commands;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Agar.IO.Server.Console.ClientConnection;

namespace Agar.IO.Server.Console.Controllers
{
    class ConnectionController
    {
        public List<ClientConnection> Connections { get; set; }
        public Action<string, BaseCommand> PlayerCommandAction { get; set; }
        public Action<string> NewPlayerAction { get; set; }

        internal async Task StartListeningAsync()
        {
            Connections = new List<ClientConnection>();
            while (true)
            {
                var newConnection = await AcceptClientAsync(IsConnectionAllowed);
                lock (Connections)
                {
                    if (Connections.Any(c => c.PlayerName == newConnection.PlayerName))
                        continue;           

                    System.Console.WriteLine($"Player {newConnection.PlayerName} has successfully connected!");
                    Connections.Add(newConnection);
                    NewPlayerAction(newConnection.PlayerName);
                }
                ProcessClientAsync(newConnection);
            }
        }

        internal void SendToClient(string name, BaseCommand com)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, com);
            stream.Seek(0, SeekOrigin.Begin);

            SendToClient(name, stream.ToArray());
        }

        public void SendToClient(string name, byte[] stream)
        {
            ClientConnection conn;
            lock (Connections)
            {
                conn = Connections.FirstOrDefault(p => p.PlayerName == name);
            }
            if (conn != null)
                conn.SendAsync(stream);
        }

        internal void EndConnection(string playerName)
        {
            EndConnection(Connections.Find(x => x.PlayerName.Equals(playerName)));
        }

        private void EndConnection(ClientConnection clientConnection)
        {
            clientConnection.IsClosed = true;
            Connections.Remove(clientConnection);
            System.Console.WriteLine($"Player stops {clientConnection.PlayerName}");
            clientConnection.Dispose();
        }

        internal void SendToAllClients(BaseCommand baseC)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, baseC);
            stream.Seek(0, SeekOrigin.Begin);

            foreach (var client in Connections)
            {
                client.SendAsync(stream.ToArray());
            }
        }

        private bool IsConnectionAllowed(string playerName, IPEndPoint playerEndPoint, out string outputMessage)
        {
            var isNameAlreadyUsed = false;

            lock (Connections)
            {
                isNameAlreadyUsed = Connections.Exists(p => p.PlayerName == playerName);
            }

            if (playerName.Length > 20) 
            {
                outputMessage = $"Name is very long! Maximum allowed length: 20";
                return false;
            }

            if (isNameAlreadyUsed)
            {
                outputMessage = $"Name {playerName} is already being used!";
                return false;
            }

            outputMessage = "";
            return true;
        }

        private async Task ProcessClientAsync(ClientConnection clientConnection)
        {
            while (!clientConnection.IsClosed)
            {
                var receiveTask = clientConnection.ReceiveCommandAsync();
                var task = await Task.WhenAny(receiveTask, Task.Delay(5000));
                if (receiveTask == task)//await Task.WhenAny(receiveTask, Task.Delay(5000)))
                {
                    var command = receiveTask.Result;
                    System.Console.WriteLine("Player {0} sent: {1}", clientConnection.PlayerName, command.GetType());
                    PlayerCommandAction(clientConnection.PlayerName, command);
                }
                else // timeout
                {
                    if (clientConnection.IsClosed)
                        return;

                    await clientConnection.SendAsync(new End());
                    System.Console.WriteLine($"Stopped player {clientConnection.PlayerName} because of timeout!");
                    PlayerCommandAction(clientConnection.PlayerName, new End());
                    return;
                }
            }
        }
    }
}
