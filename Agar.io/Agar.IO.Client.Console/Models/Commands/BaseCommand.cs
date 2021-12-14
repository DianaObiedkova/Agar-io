using ProtoBuf;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    [ProtoInclude(1, typeof(Invalid))]
    [ProtoInclude(2, typeof(Move))]
    [ProtoInclude(3, typeof(End))]
    [ProtoInclude(4, typeof(UpdateState))]
    abstract class BaseCommand
    {
        public virtual void Execute(Game game) { }
    }
}
