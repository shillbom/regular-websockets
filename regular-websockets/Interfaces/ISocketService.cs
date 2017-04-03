using RegularWebsockets.Events;

namespace RegularWebsockets.Interfaces
{
    public interface ISocketService
    {
        void OnOpen(OpenEvent ev);
        void OnClose(CloseEvent ev);
    }
}
