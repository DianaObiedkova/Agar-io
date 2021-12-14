using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Server.Console.Models
{
    [ProtoContract]
    public class Food:Entity
    {
        public Food(float x, float y, int weight)
        {
            X = x;
            Y = y;
            Weight = weight;
            Color = new byte[3];
            Server.random.NextBytes(Color);
        }
    }
}
