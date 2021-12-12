using Agar.IO.Server.Console.Models.Commands;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Server.Console.Controllers
{
    class ConnectionController
    {
        public List<ClientConnection> Connections { get; set; }
        public Action<string, BaseCommand> PlayerCommandAction { get; set; }
        public Action<string> NewPlayerAction { get; set; }
        internal async Task SendToClient(string name, BaseCommand com)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, com);
            stream.Seek(0, SeekOrigin.Begin);

            ClientConnection con = Connections.FirstOrDefault(x => x.PlayerName.Equals(name));
            if (!(con is null))  await con.SendAsync(stream.ToArray());
        }

        internal void EndConnection(string playerName)
        {
            EndConnection(Connections.Find(x => x.PlayerName.Equals(playerName)));
        }

        private void EndConnection(ClientConnection clientConnection)
        {
            clientConnection.IsClosed = true;
            Connections.Remove(clientConnection);
            //Console.WriteLine($"Player stops {clientConnection.PlayerName}");
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
    }
}
