using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RegularWebsockets.Events;

namespace Example.Service
{
    [RegularWebsockets("/greet")]
    public class GreeterService : ISocketService
    {
        public EventHandler<OpenEvent> OnOpen => throw new NotImplementedException();

        public EventHandler<CloseEvent> OnClose => throw new NotImplementedException();
    }
}
