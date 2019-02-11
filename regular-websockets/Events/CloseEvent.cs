using System.Net.WebSockets;

namespace RegularWebsockets.Events
{
    public class CloseEvent
    {
        public WebSocket Socket { get; set; }
        public WebSocketCloseStatus Reason { get; set; }
    }
}
