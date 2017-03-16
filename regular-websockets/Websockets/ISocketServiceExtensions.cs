using Microsoft.Extensions.DependencyInjection;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegularWebsockets.Websockets
{
    public static class ISocketServiceExtensions
    {
        public static void ConfigureWebSockets(this IServiceCollection services)
        {
            services.AddSingleton<ISocketService, SocketService>();
        }
    }
}
