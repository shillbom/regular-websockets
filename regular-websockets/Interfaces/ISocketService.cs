using RegularWebsockets.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegularWebsockets.Interfaces
{
    public interface ISocketService
    {
        EventHandler<OpenEvent> OnOpen { get; }
        EventHandler<CloseEvent> OnClose { get; }
    }
}
