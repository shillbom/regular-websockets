using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using RegularWebsockets.Events;

namespace RegularWebsockets.Websockets
{
    internal class SocketService: ISocketService
    {
        public SocketService()
        {
            SocketHandler.OnOpen += Open;
            SocketHandler.OnClose += Close;
        }

        private void Open(object sender, OpenEvent e)
        {
            OnOpen(this, e);
        }

        private void Close(object sender, CloseEvent e)
        {
            OnClose(this, e);
        }  

        public EventHandler<OpenEvent> OnOpen { get; set; }

        public EventHandler<CloseEvent> OnClose { get; set; }
    }
}
