using RegularWebsockets.Interfaces;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;
using RegularWebsockets.Events;

namespace Sample.Service
{
    public class GreeterService
    {
        private static IList<WebSocket> clients = new List<WebSocket>();

        public void RegisterClient(WebSocket client)
        {
            lock (clients)
            {
                clients.Add(client);
            }
        }

        public void UnRegisterClient(WebSocket client)
        {
            lock (clients)
            {
                clients.Remove(client);
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
