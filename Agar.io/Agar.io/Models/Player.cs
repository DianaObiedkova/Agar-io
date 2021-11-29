using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Player : EdibleEntity
    {
        public double Speed { get; }
        public int Score { get; set; }
        
        public Player(string id, string name) : base(id, name)
        {
            Score = 0;
            Weight = 1;
            Speed = 100;
            Location = new Position();
        }
    }
}
