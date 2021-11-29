using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public abstract class EdibleEntity
    {
        public string Id { get; }
        public string Name { get; }
        public Position Location { get; set; }
        public int Weight { get; set; }
        public double Radius => Math.Sqrt(Weight / Math.PI);

        protected EdibleEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        protected EdibleEntity(string id, string name, Position location, int weight)
        {
            Id = id;
            Name = name;
            Location = location;
            Weight = weight;
        }
    }
}
