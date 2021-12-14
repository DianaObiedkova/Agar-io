using ProtoBuf;
using System;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    class UpdateState : BaseCommand
    {
        [ProtoMember(1)]
        public GameState GameState { get; set; }
        public UpdateState(GameState gameState)
        {
            GameState = gameState;
        }
        internal override void Execute(Server server, string name)
        {
            throw new NotImplementedException();
        }
    }
}
