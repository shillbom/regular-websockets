using RegularWebsockets.Interfaces;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using RegularWebsockets.Events;

namespace Sample.Service
{
    public class GreeterService
    {
        private IList<WebSocket> clients;
        public GreeterService()
        {
            this.clients = new List<WebSocket>();
        }

        public void RegisterClient(WebSocket client)
        {
            lock (clients)
            {
                this.clients.Add(client);

            }
        }

        public void UnRegisterClient(WebSocket client)
        {
            lock (clients)
            {
                this.clients.Remove(client);
            }
        }

        public async Task SendToAll(string message)
        {
            foreach(var client in clients)
            {
                await client.SendAsync(message);
            }
        }
    }
}
