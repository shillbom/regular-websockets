using RegularWebsockets.Events;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace RegularWebsockets.Interfaces
{
    public interface IWebSocket
    {
        Task SendAsync(string text);
        Task CloseAsync();
        Task CloseAsync(WebSocketCloseStatus status, string message);
        EventHandler<RecieveEvent> OnMessage { get; set; }
        EventHandler<CloseEvent> OnClose { get; set; }
    }
}
