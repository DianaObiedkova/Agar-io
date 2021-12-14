using Agar.IO.Client.WinForms.Forms;
using Agar.IO.Client.WinForms.Models;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace Agar.IO.Client.WinForms.Controllers
{
    class LoginController
    {
        private LoginForm loginForm;
        private IPAddress ServerAddress;

        public LoginController(LoginForm loginForm)
        {
            this.loginForm = loginForm;
        }

        public async Task StartGame()
        {
            ServerConnection con;
            string name = loginForm.tbLogin.Text;
            ServerAddress = IPAddress.Loopback;

            try
            {
                con = await ServerConnection.ConnectAsync(ServerAddress, name);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return;
            }

            var game = new Game();
            var gameForm = new GameForm();
            var graph = new Graphics(gameForm);
            var input = new InputController(game, gameForm);

            game.Start(this, graph, input, con, name);
            loginForm.Visible = false;
        }

        internal void ShowMessage(string message)
        {
            loginForm.Visible = true;
            loginForm.infoLabel.Text = message;
        }
    }
}
