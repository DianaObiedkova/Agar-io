using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    internal abstract class BaseCommand
    {
        internal abstract void Execute(Server server, string name);
    }
}
