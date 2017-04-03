using Microsoft.AspNetCore.Http;
using RegularWebsockets.Interfaces;

namespace RegularWebsockets.Events
{
    public class OpenEvent
    {
        public IWebSocket Socket { get; set; }
        public HttpRequest Request { get; set; }
    }
}
