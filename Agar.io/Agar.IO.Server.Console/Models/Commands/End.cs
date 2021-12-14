using ProtoBuf;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    class End:BaseCommand
    {
        [ProtoMember(1)]
        public string Message { get; set; }
        internal override void Execute(Server server, string playerName)
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
