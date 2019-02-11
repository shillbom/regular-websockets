using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using RegularWebsockets.Events;
using System;

namespace Sample.Service
{
    [RegularWebsockets("/greet")]
    public class Greet : ISocketService
    {
        private GreeterService greetingService;

        public Greet(GreeterService greetingService)
        {
            this.greetingService = greetingService;
        }

        public async void OnOpen(OpenEvent ev)
        {
            this.greetingService.RegisterClient(ev.Socket);
            await ev.Socket.SendAsync("Welcome! Everything you say will be seen by everyone");
        }

        private void OnLeave(object sender, CloseEvent e)
        {
            Console.WriteLine("goodbye...");
        }

        public void OnClose(CloseEvent ev)
        {
            this.greetingService.UnRegisterClient(ev.Socket);
        }

        public async void OnMessage(RecieveEvent e)
        {
            await this.greetingService.SendToAll($"someone says {e.Message}");
        }

        public void Dispose()
        {
        }
    }
}
