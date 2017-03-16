using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
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

        public RegularWebSocket(WebSocket socket) 
        {
            _socket = socket;
        }

        public EventHandler<RecieveEvent> OnMessage { get; set; }

        public async Task CloseAsync(WebSocketCloseStatus status = WebSocketCloseStatus.Empty, string message = "")
        {
            await _socket.CloseAsync(status, message, CancellationToken.None);
        }

        public async Task SendAsync(string message)
        {
            if (_socket.State != WebSocketState.Open)
                return;

            var serializedMessage = JsonConvert.SerializeObject(message);
            await _socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(serializedMessage)), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public void NotifyRecieved(RecieveEvent evt)
        {
            OnMessage(this, evt);
        }

        public Task CloseAsync()
        {
            throw new NotImplementedException();
        }
    }
}
