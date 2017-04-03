using RegularWebsockets.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Service
{
    public class GreeterService
    {
        private IList<IWebSocket> clients;
        public GreeterService()
        {
            this.clients = new List<IWebSocket>();
        }

        public void RegisterClient(IWebSocket client)
        {
            lock (clients)
            {
                this.clients.Add(client);

            }
        }

        public void UnRegisterClient(IWebSocket client)
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
