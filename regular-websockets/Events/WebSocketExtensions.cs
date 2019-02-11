using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegularWebsockets.Events {
    public static class WebsocketExtensions {
        public static async Task SendAsync(this WebSocket socket, string message) {
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}