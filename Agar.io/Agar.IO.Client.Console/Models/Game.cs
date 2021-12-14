using Agar.IO.Client.WinForms.Controllers;
using Agar.IO.Client.WinForms.Handlers;
using Agar.IO.Client.WinForms.Models.Commands;
using System;
using System.Diagnostics;
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
        public GameState GameState { get; set; }
        public bool IsRunning { get; private set; }
        public bool IsPredictionValid { get; set; }

        public const int FieldWidth = 3000;

        public const int FieldHeight = 3000;
        public const int Interval = 16;

        public long Time;

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

            IsRunning = true;
            Time = -1;
            StartLoop();
        }

        internal void OnCommandReceived(BaseCommand com)
        {
            lock (this)
            {
                com.Execute(this);
            }
        }

        private async Task StartLoop()
        {
            await Task.Factory.StartNew(Loop, TaskCreationOptions.LongRunning);
        }

        private void Loop()
        {
            long a = 0;

            while (true)
            {
                if (!IsRunning)
                    break;
                long b = Stopwatch.GetTimestamp();
                long delta = 1000 * (b - a) / Stopwatch.Frequency;
                if (delta < Interval) continue;

                if (GameState?.CurrentPlayer != null)
                {
                    new MovementHandler(InputContr.MousePosition).Execute(this);

                    var gameStateForRendering = GameState.DeepClonePrediction();
                    Graph.Render(gameStateForRendering);
                }
                a = Stopwatch.GetTimestamp();
            }
        }
    }
}
