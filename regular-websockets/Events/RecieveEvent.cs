using System.Net.WebSockets;

namespace RegularWebsockets.Events
{
    public class RecieveEvent
    {
        public WebSocketMessageType MessageType { get; set; }
        public string Message;
    }
}
