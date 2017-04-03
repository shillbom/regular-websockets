using RegularWebsockets.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegularWebsockets.Interfaces
{
    public interface ISocketService
    {
        void OnOpen(OpenEvent ev);
        void OnClose(CloseEvent ev);
    }
}
