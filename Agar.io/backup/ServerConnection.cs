class ServerConnection: IDisposable
    {
        private static int LoginServerPort = 11028;
        private UdpClient UdpServer;
        private bool IsClosed { get; set; }

        public static async Task<ServerConnection> ConnectAsync(IPAddress address, string playerName)
        {
            var con = new ServerConnection
            {
                UdpServer = new UdpClient(new IPEndPoint(IPAddress.Any, 0))
            };
            con.UdpServer.Connect(address, LoginServerPort);
            con.IsClosed = false;

            Debug.WriteLine("CONNECTING");
            var task = con.ReceiveAsync();
            string result;

            for(int i = 0; i < 5; i++)
            {
                await con.SendAsync($"CONNECT {playerName}");

                if(await Task.WhenAny(task, Task.Delay(1000)) == task)
                {
                    result = task.Result;
                    

                    switch (result.Split()[0])
                    {
                        case "CONNECTED":
                            con.UdpServer.Connect(address, int.Parse(result.Split()[1]));
                            for (int j = 0; j < 3; j++) 
                                con.SendAsync("OK!");
                            //con.UdpServer.Dispose();
                            return con;
                        case "ERROR":
                            con.Dispose();
                            throw new ArgumentException(result.Substring(6).Trim());
                    }
                }
            }

            con.Dispose();
            throw new TimeoutException("Cannot connect to the server!");
        }

        internal async Task SendAsync(BaseCommand com)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, com);
            stream.Seek(0, SeekOrigin.Begin);
            var res = UdpServer.SendAsync(stream.ToArray(), stream.ToArray().Length);
            await res;

            //Debug.WriteLine("Sending {0}", command.GetType());

            if (res.Exception != null)
                Debug.WriteLine(res.Exception);
        }

        internal async void StartReceiving(Action<BaseCommand> onCommandReceived)
        {
            while (true)
            {
                if (IsClosed)
                    break;
                var task = ReceiveCommandAsync();
                if(await Task.WhenAny(task, Task.Delay(1000)) == task)
                {
                    onCommandReceived(task.Result);
                }
                else
                {
                    onCommandReceived(new End("The server no longer responds"));
                }
            }
        }

        internal async Task<BaseCommand> ReceiveCommandAsync()
        {
            while (true)
            {
                var data = await ReceiveBinaryAsync();
                var stream = new MemoryStream(data);
                try
                {
                    return Serializer.Deserialize<BaseCommand>(stream);
                }
                catch (ProtoException)
                {
                    // ignore this type of exception (multiple ACK ...), wait for first command
                }
            }
        }

        private async Task SendAsync(string v)
        {
            var arr = Encoding.UTF8.GetBytes(v);
            await UdpServer.SendAsync(arr, arr.Length);
        }

        private async Task<string> ReceiveAsync()
        {
            var res = await ReceiveBinaryAsync();
            return Encoding.UTF8.GetString(res);
        }

        public async Task<byte[]> ReceiveBinaryAsync()
        {
            return (await UdpServer.ReceiveAsync()).Buffer;
        }

        public void Dispose()
        {
            UdpServer.Dispose();
            IsClosed = true;
        }
    }