using Agar.IO.Server.Console;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocket.Test
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var game = new Server();
            game.Start();
            //await WebSocketOperation();
        }

        static async Task WebSocketOperation()
        {
            Console.WriteLine("press enter to continue...");
            Console.ReadLine();

            using (ClientWebSocket client = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("ws://localhost:5000/send");
                var cTs = new CancellationTokenSource();
                cTs.CancelAfter(TimeSpan.FromSeconds(60));
                try
                {
                    await client.ConnectAsync(serviceUri, cTs.Token);
                    var n = 0;
                    while (client.State == WebSocketState.Open)
                    {
                        Console.WriteLine("Enter message to send: ");
                        string message = Console.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> bytesToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(bytesToSend, WebSocketMessageType.Text, true, cTs.Token);
                            var responseBuffer = new byte[1024];
                            var offset = 0;
                            var packet = 1024;
                            while (true)
                            {
                                ArraySegment<byte> bytesReceived = new ArraySegment<byte>(responseBuffer, offset, packet);
                                WebSocketReceiveResult response = await client.ReceiveAsync(bytesReceived, cTs.Token);
                                var responseMessage = Encoding.UTF8.GetString(responseBuffer, offset, response.Count);
                                Console.WriteLine(responseMessage);
                                if (response.EndOfMessage) break;
                            }
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            Console.ReadLine();
        }
    }
}
