using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Models.Commands
{
    [ProtoContract]
    class Move : BaseCommand
    {
        [ProtoMember(1)]
        public List<Tuple<int, float, float, float>> Movement { get; set; }
        [ProtoMember(2)]
        public long Time { get; set; }

        public override void Execute(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
