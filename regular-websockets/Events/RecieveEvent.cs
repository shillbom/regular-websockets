using System.Net.WebSockets;

namespace RegularWebsockets.Events
{
    public class RecieveEvent
    {
        public WebSocketMessageType MessageType { get; set; }
        public WebSocket socket { get; set; }
        public string Message { get; set; }
    }
}
