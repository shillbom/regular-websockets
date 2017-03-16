using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace RegularWebsockets.Events
{
    public class RecieveEvent
    {
        public WebSocketMessageType MessageType { get; set; }
        public string Message;
    }
}
