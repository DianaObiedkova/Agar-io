using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Server.Console
{
    class ServerLogin : UdpClient
    {
        public readonly static int serverPort = 11028; //https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.udpclient?view=net-6.0
        ServerLogin(IPEndPoint endpoint) : base(endpoint) { }
        public static ServerLogin NewInstance()
        {
            return new ServerLogin(new IPEndPoint(IPAddress.Any, serverPort));
        }
        public async Task SendAsync(string mes)
        {
            byte[] arr = Encoding.UTF8.GetBytes(mes);
            await SendAsync(arr, arr.Length);
        }
    }
}
