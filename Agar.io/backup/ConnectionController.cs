class ConnectionController
    {
        public List<ClientConnection> Connections { get; set; }
        public Action<string, BaseCommand> PlayerCommandAction { get; set; }
        public Action<string> NewPlayerAction { get; set; }

        internal async Task StartListeningAsync()
        {
            Connections = new List<ClientConnection>();
            while (true)
            {
                var newConnection = await AcceptClientAsync(IsConnectionAllowed);
                lock (Connections)
                {
                    if (Connections.Any(c => c.PlayerName == newConnection.PlayerName))
                        continue;           // already connected (... multiple connect packets from client)

                    //Console.WriteLine($"Player {newConnection.PlayerName} has successfully connected!");
                    Connections.Add(newConnection);
                    NewPlayerAction(newConnection.PlayerName);
                }
                await ProcessClientAsync(newConnection);
            }
        }

        internal async Task SendToClient(string name, BaseCommand com)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, com);
            stream.Seek(0, SeekOrigin.Begin);

            ClientConnection con = Connections.FirstOrDefault(x => x.PlayerName.Equals(name));
            if (!(con is null))  await con.SendAsync(stream.ToArray());
        }

        internal void EndConnection(string playerName)
        {
            EndConnection(Connections.Find(x => x.PlayerName.Equals(playerName)));
        }

        private void EndConnection(ClientConnection clientConnection)
        {
            clientConnection.IsClosed = true;
            Connections.Remove(clientConnection);
            //Console.WriteLine($"Player stops {clientConnection.PlayerName}");
            clientConnection.Dispose();
        }

        internal void SendToAllClients(BaseCommand baseC)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, baseC);
            stream.Seek(0, SeekOrigin.Begin);

            foreach (var client in Connections)
            {
                client.SendAsync(stream.ToArray());
            }
        }

        private bool IsConnectionAllowed(string playerName, IPEndPoint playerEndPoint, out string outputMessage)
        {
            var isNameAlreadyUsed = false;

            lock (Connections)
            {
                isNameAlreadyUsed = Connections.Exists(p => p.PlayerName == playerName);
            }

            if (playerName.Length > 20) // maximum player name length
            {
                outputMessage = $"Name is too long! Maximum allowed name length is 20.";
                return false;
            }

            if (isNameAlreadyUsed)
            {
                outputMessage = $"Name {playerName} is already being used by another player!";
                return false;
            }

            outputMessage = "";
            return true;
        }

        private async Task ProcessClientAsync(ClientConnection clientConnection)
        {
            while (!clientConnection.IsClosed)
            {
                var receiveTask = clientConnection.ReceiveCommandAsync();
                var task = await Task.WhenAny(receiveTask, Task.Delay(5000));
                if (task == receiveTask)
                {
                    var command = receiveTask.Result;
                    //Console.WriteLine("Player {0} sent: {1}", clientConnection.PlayerName, command.GetType());
                    PlayerCommandAction(clientConnection.PlayerName, command);
                }
                else // timeout
                {
                    if (clientConnection.IsClosed)
                        return;

                    await clientConnection.SendAsync(new End());
                    //Console.WriteLine($"Stopped player {clientConnection.PlayerName} because of timeout!");
                    PlayerCommandAction(clientConnection.PlayerName, new End());
                    return;
                }
            }
        }
    }