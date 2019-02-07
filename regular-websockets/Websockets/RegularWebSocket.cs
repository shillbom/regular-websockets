using RegularWebsockets.Interfaces;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using RegularWebsockets.Events;
using System.Threading;
using Newtonsoft.Json;

namespace RegularWebsockets.Websockets
{
    internal class RegularWebSocket : IWebSocket
    {
        private WebSocket _socket;

        public RegularWebSocket(WebSocket socket) => _socket = socket;
        public EventHandler<RecieveEvent> OnMessage { get; set; }
        public EventHandler<CloseEvent> OnClose { get; set; }

        public WebSocketState Status => _socket.State;

        public async Task SendAsync(string message)
        {
            if (_socket.State != WebSocketState.Open)
                return;

            await _socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(message)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void NotifyRecieved(RecieveEvent evt) => OnMessage(this, evt);
        public void NotifyClosed(CloseEvent evt) => OnClose(this, evt);

        public async Task CloseAsync() => await CloseAsync(WebSocketCloseStatus.Empty, "");
        public async Task CloseAsync(WebSocketCloseStatus status, string message) => await _socket.CloseAsync(status, message, CancellationToken.None);
        public void Dispose() => this._socket.Dispose();
    }
}
