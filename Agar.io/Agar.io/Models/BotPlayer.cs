using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agar.io.Models
{
    public class BotPlayer: Player
    {
        private DateTime newUpdate;
        Random ran = new Random();
        public BotPlayer(string id, string name): base(id, name) { }

        public override bool UpdateMove(int newX, int newY)
        {
            if (newUpdate <= DateTime.Now)
            {
                Direction = new Vector
                {
                    X = ran.Next(-2, 2) * ran.NextDouble(),
                    Y = ran.Next(-2, 2) * ran.NextDouble()
                };

                CreateNewUpdate();
            }

            return base.UpdateMove(newX, newY);
        }

        private void CreateNewUpdate()
        {
            newUpdate = DateTime.Now.AddMilliseconds(ran.Next(2000));
        }
    }
}
