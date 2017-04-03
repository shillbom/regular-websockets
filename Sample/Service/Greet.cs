using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using RegularWebsockets.Events;

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

        public void OnOpen(OpenEvent ev)
        {
            ev.Socket.OnMessage += OnMessage;

            this.greetingService.RegisterClient(ev.Socket);
        }
        public void OnClose(CloseEvent ev)
        {
            this.greetingService.UnRegisterClient(ev.Socket);
        }

        private async void OnMessage(object sender, RecieveEvent e)
        {
            await this.greetingService.SendToAll($"say hi to {e.Message}");
        }        
    }
}
