using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq;
using RegularWebsockets.Events;

namespace Sample.Service
{
    [RegularWebsockets("/greet")]
    public class Greet : ISocketService
    {
        private IList<IWebSocket> clients;

        public Greet()
        {
            this.clients = new List<IWebSocket>();
        }

        public void OnOpen(OpenEvent ev)
        {
            ev.Socket.OnMessage += OnMessage;

            lock(clients)
            {
                clients.Add(ev.Socket);
            }
        }
        public void OnClose(CloseEvent ev)
        {
            lock(clients)
            {
                clients.Remove(ev.Socket);
            }
        }

        private void OnMessage(object sender, RecieveEvent e)
        {
            foreach(var client in clients)
            {
                client.SendAsync($"Say hello to {e.Message}");
            }
        }        
    }
}
