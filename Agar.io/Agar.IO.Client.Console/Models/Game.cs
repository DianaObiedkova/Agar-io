using Agar.IO.Client.WinForms.Controllers;
using Agar.IO.Client.WinForms.Models.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Client.WinForms.Models
{
    class Game
    {
        public string PlayerName { get; set; }
        public ServerConnection ServerConnection { get; set; }
        public LoginController LoginContr { get; set; }
        private InputController InputContr { get; set; }
        private Graphics Graph { get; set; }
        private bool IsRunning;

        internal void Close(string message)
        {
            IsRunning = false;
            ServerConnection.SendAsync(new End(message)).ContinueWith(new Action<Task>(t =>
            {
                ServerConnection.Dispose();
                Debug.WriteLine("stopped");
            }));

            Graph.StopGraph();
            LoginContr.ShowMessage(message);
        }

        internal void Start(LoginController loginController, Graphics graph, InputController input, ServerConnection con, string name)
        {
            LoginContr = loginController;
            Graph = graph;
            InputContr = input;
            ServerConnection = con;
            PlayerName = name;

            Graph.StartGraph();
            ServerConnection.StartReceiving(OnCommandReceived);
        }

        internal void OnCommandReceived(BaseCommand com)
        {
            com.Execute(this);
        }
    }
}
