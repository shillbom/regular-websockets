using Microsoft.AspNetCore.Http;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace RegularWebsockets.Events
{
    public class OpenEvent
    {
        public IWebSocket Socket { get; set; }
        public HttpRequest Request { get; set; }
    }
}
