using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public abstract class Entity
    {
        public string Id { get; }
        public string Name { get; }

        protected Entity(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
