using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Position
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Position(int X, int Y)
        {
            this.X = X;
            this.Y = Y;
        }
        public Position()
        {
            Random ran = new Random();
            X = ran.Next(int.MaxValue); //we could limit it for our future needs
            Y = ran.Next(int.MaxValue);
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 3049; // Start value (prime number).

                // Suitable nullity checks etc, of course :)
                hash = hash * 5039 + X.GetHashCode();
                hash = hash * 883 + Y.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            var other = (Position)obj;

            if (other == null)
                return false;

            return X == other.X && Y == other.Y;
        }
    }
}
