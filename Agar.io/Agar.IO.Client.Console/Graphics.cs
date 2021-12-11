using Agar.IO.Client.WinForms.Forms;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agar.IO.Client.WinForms
{
    class Graphics
    {
        private GameForm gameForm;

        public Graphics(GameForm gameForm)
        {
            this.gameForm = gameForm;
        }

        internal void StopGraph()
        {
            gameForm.Close();
        }

        internal void StartGraph()
        {
            gameForm.Show();
        }
    }
}
