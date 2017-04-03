using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegularWebsockets.Events;

namespace Sample.Service
{
    [RegularWebsockets("/echo")]
    public class Echo : ISocketService
    {
        public void OnOpen(OpenEvent ev)
        {
            ev.Socket.OnMessage += OnMessage;
        }
        public void OnClose(CloseEvent ev)
        {
            throw new NotImplementedException();
        }

        private void OnMessage(object sender, RecieveEvent e)
        {
            (sender as IWebSocket).SendAsync(e.Message);
        }        
    }
}
