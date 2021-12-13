using Agar.IO.Client.WinForms.Forms;
using Agar.IO.Client.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Agar.IO.Client.WinForms.Controllers
{
    class InputController
    {
        private Game game;
        private GameForm gameForm;
        public Position MousePosition { get; private set; }

        public InputController(Game game, GameForm gameForm)
        {
            this.game = game;
            this.gameForm = gameForm;

            gameForm.FormClosed += GameForm_FormClosed; 
            gameForm.GamePanel.Paint += GamePanel_Paint;
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            if (gameForm.GamePanel.Focused || gameForm.Focused) // TODO - remove condition - just for testing
                UpdateMousePosition();
        }

        private void UpdateMousePosition()
        {
            double posX = gameForm.GamePanel.PointToClient(Cursor.Position).X;
            double posY = gameForm.GamePanel.PointToClient(Cursor.Position).Y;
            if (game.GameState == null)
                return;

            // view to game coefficient
            float q = (float)((0.1 * gameForm.GamePanel.Width) / game.GameState.CurrentPlayer.Radius);

            // opposite order!
            posX -= gameForm.GamePanel.Width / 2.0f;
            posY -= gameForm.GamePanel.Height / 2.0f;
            if (q < 1)
            {
                posX *= 1 / q;
                posY *= 1 / q;
            }
            posX += game.GameState.CurrentPlayer.X;
            posY += game.GameState.CurrentPlayer.Y;

            MousePosition = new Position(posX, posY);
        }


        private void GameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (game.IsRunning)
                game.Close("");
        }
    }
}
