using Agar.IO.Client.WinForms.Forms;
using Agar.IO.Client.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms.Controllers
{
    class InputController
    {
        private Game game;
        private GameForm gameForm;

        public InputController(Game game, GameForm gameForm)
        {
            this.game = game;
            this.gameForm = gameForm;
        }
    }
}
