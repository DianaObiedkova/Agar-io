using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(Invalid))]
    [ProtoInclude(2, typeof(Move))]
    [ProtoInclude(3, typeof(End))]
    [ProtoInclude(4, typeof(UpdateState))]
    abstract class BaseCommand
    {
        public abstract void Execute(Game game);
    }
}
