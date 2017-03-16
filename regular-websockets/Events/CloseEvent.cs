using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace RegularWebsockets.Events
{
    public class CloseEvent
    {
        public IWebSocket Socket { get; set; }
        public WebSocketCloseStatus Reason { get; set; }
    }
}
