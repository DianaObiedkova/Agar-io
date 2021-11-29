using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Player : EdibleEntity
    {
        public double MaxSpeed { get; }
        public int Score { get; set; }
        public Vector Direction { get; set; }
        public Player(string id, string name) : base(id, name)
        {
            Score = 0;
            Weight = 1;
            MaxSpeed = 10;
            Location = new Position();
            Direction = new Vector();
        }

        public virtual bool UpdateMove(int newX,int newY)
        {
            var speed = Direction.Length;
            if (speed == 0)  return false;

            if (speed > MaxSpeed) speed = MaxSpeed;

            var updated = Vector.Normalize(Direction, speed);

            var newLocation = new Position(Location.X + updated.X, Location.Y + updated.Y);
            
            if (newLocation.X < 0) newLocation.X = 0;
            else if (newLocation.X > newX) newLocation.X = newX;

            if (newLocation.Y < 0) newLocation.Y = 0;
            else if (newLocation.Y > newY) newLocation.Y = newY;

            Location = newLocation;
            return true;
        }
    }
}
