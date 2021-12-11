using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    class End:BaseCommand
    {
        public string Message { get; set; }
        public override void Execute(Server server, string playerName)
        {
            server.RemovePlayer(playerName, Message);
        }
        public End() { }
        public End(string Message)
        {
            this.Message = Message;
        }
    }
}
