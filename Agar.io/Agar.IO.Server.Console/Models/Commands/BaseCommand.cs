using ProtoBuf;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(Invalid))]
    [ProtoInclude(2, typeof(Move))]
    [ProtoInclude(3, typeof(End))]
    [ProtoInclude(4, typeof(UpdateState))]
    internal abstract class BaseCommand
    {
        internal abstract void Execute(Server server, string name);
    }
}
