using Agar.IO.Client.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Handlers
{
    abstract class Handler
    {
        public float X { get; set; }
        public float Y { get; set; }

        protected Handler(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
        }
        public abstract void Execute(Game game);
    }
}
