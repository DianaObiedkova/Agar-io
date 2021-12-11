using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Client.WinForms
{
    class ServerConnection:IDisposable
    {
        private static int LoginServerPort = 11000;
        private UdpClient UdpServer;
        private bool IsClosed { get; set; }

        public static async Task<ServerConnection> ConnectAsync(IPAddress address, string playerName)
        {
            var con = new ServerConnection
            {
                UdpServer = new UdpClient(new IPEndPoint(IPAddress.Any, 0)),
                IsClosed = false
            };
            con.UdpServer.Connect(address, LoginServerPort);

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
                            con.UdpServer.Connect(address, int.Parse(result.Split()[0]));
                            for (int j = 0; j < 3; j++) await con.SendAsync("OK!");
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
}
