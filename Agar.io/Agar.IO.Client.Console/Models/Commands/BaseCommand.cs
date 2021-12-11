using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    abstract class BaseCommand
    {
        public abstract void Execute(Game game);
    }
}
