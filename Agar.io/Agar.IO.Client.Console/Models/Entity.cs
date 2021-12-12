using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Models
{
    
    [ProtoContract]
    [ProtoInclude(5, typeof(Food))]
    [ProtoInclude(6, typeof(Player))]
    public abstract class Entity
    {
        [ProtoMember(1)]
        public int Weight { get; set; }
        [ProtoIgnore]
        public double Radius => 10 * Math.Sqrt(Weight / Math.PI);
        [ProtoMember(2)]
        public double X { get; set; }
        [ProtoMember(3)]
        public double Y { get; set; }
        [ProtoMember(4)]
        public byte[] Color { get; set; }
    }
}
