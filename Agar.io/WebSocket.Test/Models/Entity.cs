using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models
{
    [ProtoContract]
    public abstract class Entity
    {
        [ProtoMember(1)]
        public int Weight { get; set; }
        public double Radius => Math.Sqrt(Weight / Math.PI);
        [ProtoMember(2)]
        public double X { get; set; }
        [ProtoMember(3)]
        public double Y { get; set; }
        [ProtoMember(4)]
        public byte[] Color { get; set; }
    }
}
