using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class FoodFactory
    {
        private long count = -1;
        private readonly int maxWidth, maxHeight;
        readonly Random ran = new Random();
        public FoodFactory(int w, int h)
        {
            maxHeight = h;
            maxWidth = w;
        }

        public Food Create()
        {
            count++;

            return new Food(count.ToString(), "Food " + count.ToString())
            {
                Location = new Position
                {
                    X = ran.Next(maxWidth),
                    Y = ran.Next(maxHeight)
                },
                Weight = ran.Next(1, 5)
            };
        }
    }
}
