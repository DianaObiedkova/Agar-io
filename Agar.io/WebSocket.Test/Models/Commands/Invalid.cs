using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models.Commands
{
    [ProtoContract]
    class Invalid:BaseCommand
    {
        public string Message { get; set; }
        public Invalid(string message)
        {
            Message = message;
        }
    }
}
