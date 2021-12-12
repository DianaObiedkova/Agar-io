using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    class Invalid:BaseCommand
    {
        [ProtoMember(1)]
        public string Message { get; set; }
        public Invalid(string message)
        {
            Message = message;
        }

        internal override void Execute(Server server, string name)
        {
            throw new NotImplementedException();
        }
    }
}
