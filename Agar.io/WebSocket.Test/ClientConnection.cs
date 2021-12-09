using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Agar.IO.Server.Console
{
    class ClientConnection
    {
        public string PlayerName { get; set; }
        public int LastUpdate { get; set; }
        UdpClient UdpClient { get; set; }

        public async Task SendAsync(string message)
        {
            byte[] arr = Encoding.UTF8.GetBytes(message);
            await UdpClient.SendAsync(arr, arr.Length);
        }

        public async Task SendAsync(byte[] message)
        {
            await UdpClient.SendAsync(message, message.Length);
        }

        public string GetMessage(UdpReceiveResult res)
        {
            return Encoding.UTF8.GetString(res.Buffer);
        }
    }
}
