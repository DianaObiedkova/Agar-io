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

        internal async Task SendToClient(string name, BaseCommand com)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, com);
            stream.Seek(0, SeekOrigin.Begin);

            ClientConnection con = Connections.FirstOrDefault(x => x.PlayerName.Equals(name));
            if (!(con is null))  await con.SendAsync(stream.ToArray());
        }
    }
}
