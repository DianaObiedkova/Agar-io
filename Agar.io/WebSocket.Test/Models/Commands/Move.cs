using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    class Move : BaseCommand
    {
        public long Time { get; set; }
        public override void Execute(Server server, string name)
        {
            throw new NotImplementedException();
        }
    }
}
