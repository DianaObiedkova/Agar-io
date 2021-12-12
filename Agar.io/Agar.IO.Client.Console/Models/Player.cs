using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Agar.IO.Client.WinForms.Models
{
    [ProtoContract]
    public class Player : Entity
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        public long FirstMovementTime { get; set; }
        public long LastMovementTime { get; set; }
        public long FirstMovementServerTime { get; set; }
        [ProtoMember(2)]
        public bool IsBeingEjected { get; set; }

    }
}
