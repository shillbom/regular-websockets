using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace RegularWebsockets.Events
{
    public class OpenEvent
    {
        public WebSocket Socket { get; set; }
        public HttpRequest Request { get; set; }
    }
}
