using RegularWebsockets.Events;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace RegularWebsockets.Interfaces
{
    public interface IWebSocket
    {
        Task SendAsync(string text);
        Task CloseAsync(WebSocketCloseStatus status, string message);
        EventHandler<RecieveEvent> OnMessage { get; set; }
    }
}
