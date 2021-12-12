using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    abstract class BaseCommand
    {
        public abstract void Execute(Server server, string name);
    }
}
