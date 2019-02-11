using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using RegularWebsockets.Events;

namespace Sample.Service
{
    [RegularWebsockets("/echo")]
    public class Echo : ISocketService
    {
        public void Dispose()
        {
        }

        public void OnClose(CloseEvent ev)
        {
        }

        public void OnOpen(OpenEvent ev)
        {
        }

        private async void OnMessage(RecieveEvent ev)
        {
            await ev.socket.SendAsync(ev.Message);
        }

        void ISocketService.OnMessage(RecieveEvent ev)
        {
        }
    }
}
