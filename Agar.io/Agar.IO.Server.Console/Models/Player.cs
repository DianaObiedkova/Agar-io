using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Agar.IO.Server.Console.Models
{
    [ProtoContract]
    public class Player : Entity
    {
        [ProtoMember(1)]
        public string Name { get; set; }
        public long FirstMovementTime { get; set; }
        public long LastMovementTime { get; set; }
        public long FirstMovementServerTime { get; set; }
        public Player(string name)
        {
            Name = name;
            X = Server.random.Next((int)Math.Round(Radius), Server.fieldWidth);
            Y = Server.random.Next((int)Math.Round(Radius), Server.fieldHeight);

            FirstMovementTime = 0;
            FirstMovementServerTime = Stopwatch.GetTimestamp();

            Color = new byte[3];
            Server.random.NextBytes(Color);
        }
    }
}
