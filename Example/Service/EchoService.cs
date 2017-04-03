using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegularWebsockets.Events;
using RegularWebsockets.Attributes;

namespace Example.Service
{
    [RegularWebsockets("/echo")]
    public class EchoService : ISocketService
    {
        public EventHandler<OpenEvent> OnOpen => throw new NotImplementedException();

        public EventHandler<CloseEvent> OnClose => throw new NotImplementedException();
    }
}
