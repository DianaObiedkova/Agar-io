using Agar.IO.Server.Models.Communication.Classes;
using Agar.IO.Server.Models.Communication.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Agar.IO.Server.Controllers
{
    public class WebSocketController : ControllerBase,ICommunicator
    {
        [HttpGet("/send")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Send(HttpContext, ws);
            }
        }

        public async void Send(Message message)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using WebSocket ws = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Send(message, ws);
            }
        }

        private async Task Send(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[4096];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Server says: {DateTime.Now:f}")), result.MessageType, true, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        private async Task Send(Message mes, WebSocket webSocket)
        {
            var buffer = new byte[4096];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(mes.ToString())), result.MessageType, true, CancellationToken.None);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
