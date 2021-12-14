using Agar.IO.Server.Console.Models.Commands;
using ProtoBuf;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Server.Console
{
    public class ClientConnection:IDisposable
    {
        public string PlayerName { get; set; }
        public int LastUpdate { get; set; }
        public UdpClient UdpClient { get; set; }
        public bool IsClosed { get; set; }
        public int LastMovementTime { get; set; }

        public delegate bool ClientAuthorizer(string playerName, IPEndPoint endPoint, out string outputMessage);

        public async Task SendAsync(string message)
        {
            byte[] arr = Encoding.UTF8.GetBytes(message);
            await UdpClient.SendAsync(arr, arr.Length);
        }

        public async Task SendAsync(byte[] message)
        {
            await UdpClient.SendAsync(message, message.Length);
        }

        internal async Task SendAsync(BaseCommand command)
        {
            var stream = new MemoryStream();
            Serializer.Serialize(stream, command);
            stream.Seek(0, SeekOrigin.Begin);
            await SendAsync(stream.ToArray());
        }


        public void Dispose()
        {
            if (UdpClient != null)
                UdpClient.Dispose();
        }

        internal async Task<BaseCommand> ReceiveCommandAsync()
        {
            while (true)
            {
                byte[] res = (await UdpClient.ReceiveAsync()).Buffer;
                var stream = new MemoryStream(res);
                try
                {
                    var command = Serializer.Deserialize<BaseCommand>(stream);
                    return command;
                }
                catch (ProtoException e) { }
            }
        }

        public static async Task<ClientConnection> AcceptClientAsync(ClientAuthorizer clientAuthorizer)
        {
            var conn = new ClientConnection();
            var loginServer = ServerLogin.NewInstance();
            conn.IsClosed = false;
            conn.UdpClient = new UdpClient(new IPEndPoint(IPAddress.Any, 0));

            while (true)
            {
                while (true)
                {
                    var connectionResult = await loginServer.ReceiveAsync();
                    var message = GetMessageFromUdpReceiveResult(connectionResult);

                    if (message.Split().Length >= 2 && message.Split()[0] == "CONNECT")
                    {
                        var name = message.Substring(8);

                        if (clientAuthorizer(name, connectionResult.RemoteEndPoint, out string authorizerOutputMessage))
                        {
                            loginServer.Connect(connectionResult.RemoteEndPoint);
                            conn.PlayerName = name;
                            conn.LastMovementTime = 0;
                            break;
                        }
                        else
                        {
                            loginServer.Connect(connectionResult.RemoteEndPoint);
                            System.Console.WriteLine("Connection not allowed for {0}:{1} with desired name {2}",
                                connectionResult.RemoteEndPoint.Address, connectionResult.RemoteEndPoint.Port, name);
                            await loginServer.SendAsync($"ERROR {authorizerOutputMessage}");
                            loginServer.Dispose();
                            loginServer = ServerLogin.NewInstance();
                        }
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    await loginServer.SendAsync("CONNECTED " + (conn.UdpClient.Client.LocalEndPoint as IPEndPoint).Port);

                    var connectionResult = conn.UdpClient.ReceiveAsync();
                    if (await Task.WhenAny(Task.Delay(1000), connectionResult) == connectionResult)
                    {
                        var message = GetMessageFromUdpReceiveResult(connectionResult.Result);

                        if (message == "OK!")
                        {
                            conn.UdpClient.Connect(connectionResult.Result.RemoteEndPoint);
                            System.Console.WriteLine($"{connectionResult.Result.RemoteEndPoint.Address}:{connectionResult.Result.RemoteEndPoint.Port} has connected!");
                            loginServer.Dispose();
                            return conn;
                        }
                    }
                }
            }
        }

        public static string GetMessageFromUdpReceiveResult(UdpReceiveResult result)
        {
            var message = Encoding.Default.GetString(result.Buffer);
            return message;
        }
    }
}
