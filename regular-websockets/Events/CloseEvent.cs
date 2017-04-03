using RegularWebsockets.Interfaces;
using System.Net.WebSockets;

namespace RegularWebsockets.Events
{
    public class CloseEvent
    {
        public IWebSocket Socket { get; set; }
        public WebSocketCloseStatus Reason { get; set; }
    }
}
