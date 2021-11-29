using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Length => Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));

        public void Normalize(double length = 1)
        {
            double ratio = Length / length;
            X /= ratio;
            Y /= ratio;
        }

        public static Vector Normalize(Vector direction, double length = 1)
        {
            var res = direction.EqualVector();
            res.Normalize(length);
            return res;
        }

        public Vector EqualVector() => new Vector { X = X, Y = Y };
    }
}
