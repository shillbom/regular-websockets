using Microsoft.Extensions.DependencyInjection;
using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RegularWebsockets.Websockets
{
    public static class ISocketServiceExtensions
    {
        public static void ConfigureWebSockets(this IServiceCollection services)
        {
            LocateServices();
        }

        private static void LocateServices()
        {
            Assembly.GetEntryAssembly()
                .GetTypes()
                .AsEnumerable()
                .Where(type => typeof(ISocketService).IsAssignableFrom(type))
                .ToList()
                .ForEach(d =>
                {
                    var pathAttribute = d.GetTypeInfo().GetCustomAttributes<RegularWebsocketsAttribute>();
                    var path = pathAttribute.FirstOrDefault().path;
                    if (!string.IsNullOrEmpty(path))
                    {
                        SocketHandler.RegisterHandler(path, d);
                    }
                });
        }
    }
}
