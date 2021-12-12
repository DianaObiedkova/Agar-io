using Agar.IO.Client.WinForms.Controllers;
using Agar.IO.Client.WinForms.Forms;
using Agar.IO.Client.WinForms.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Agar.IO.Client.WinForms
{
    class Graphics
    {
        private GameForm gameForm;
        private static PictureBox GamePanel { get; set; }
        private GameState GameStateCopy { get; set; }
        private Pen Pen { get; set; }

        public Graphics(GameForm gameForm)
        {
            this.gameForm = gameForm;
            GamePanel = gameForm.GamePanel;
            Pen = new Pen(Color.FromArgb(20, 20, 20), 10);
            GamePanel.Paint += GamePanel_Paint;
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            if (GameStateCopy == null)
                return;
            DrawGame(e.Graphics);
        }

        private void DrawGame(System.Drawing.Graphics graphics)
        {
            if (GameStateCopy?.CurrentPlayer is null) return;
            graphics.Clear(Color.DarkGray);
            DrawMatrix(graphics);
            DrawFood(graphics);
            DrawPlayers(graphics);
            DrawScores(graphics);
        }

        private void DrawScores(System.Drawing.Graphics g)
        {
            if (GameStateCopy?.Players == null)
                return;

            var scoreText = new StringBuilder();
            var sortedPlayers = (from player in GameStateCopy.Players
                                 orderby player.Weight descending
                                 select player).ToList();

            for (int i = 0; i < sortedPlayers.Count; i++)
                scoreText.Append($"\n{i + 1}. {sortedPlayers[i].Name} : {sortedPlayers[i].Weight}");

            gameForm.labelScores.Text = "Leaderboard" + scoreText.ToString();
        }

        private void DrawPlayers(System.Drawing.Graphics g)
        {
            var playerssWithColorAndName = from player in GameStateCopy.Players select new { player.Color, PlayerName = player.Name, player.Radius, player.X, player.Y, player.IsBeingEjected };
            foreach (var player in playerssWithColorAndName)
            {
                var radius = player.Radius;
                var color = player.Color;
                var name = player.PlayerName;

                var brush = new SolidBrush(Color.FromArgb(color[0], color[1], color[2]));

                g.FillEllipse(brush, (float)(player.X - radius),
                   (float)(player.Y - radius), (float)(2 * radius), (float)(2 * radius));

                if (!player.IsBeingEjected) // рисуем имя игрока
                {
                    var myFont = new Font("Arial", 14);
                    var sizeOfText = g.MeasureString(name, myFont);
                    g.DrawString(name, myFont, Brushes.Black, (float)(player.X - sizeOfText.Width / 2), (float)(player.Y - sizeOfText.Height / 2));
                }
            }
        }

        private void DrawFood(System.Drawing.Graphics g)
        {
            if (GameStateCopy.FoodList == null)
                return;

            foreach (var food in GameStateCopy.FoodList)
            {
                var brush = new SolidBrush(Color.FromArgb(food.Color[0], food.Color[1], food.Color[2]));
                g.FillEllipse(brush, (float)(food.X - food.Radius),
                   (float)(food.Y - food.Radius), (float)(2 * food.Radius), (float)(2 * food.Radius));
            }
        }

        private void DrawMatrix(System.Drawing.Graphics g)
        {
            for (int x = 0; x <= Game.FieldWidth; x += 100)
                g.DrawLine(Pen, x, 0, x, Game.FieldWidth);

            for (int y = 0; y <= Game.FieldHeight; y += 100)
                g.DrawLine(Pen, 0, y, Game.FieldHeight, y);
        }

        internal void StopGraph()
        {
            gameForm.Close();
        }

        internal void StartGraph()
        {
            gameForm.Show();
        }

        internal void Render(GameState state)
        {
            GameStateCopy = state;
            GamePanel.Invalidate();
        }
    }
}
