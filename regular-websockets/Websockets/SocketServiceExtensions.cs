using Microsoft.Extensions.DependencyInjection;
using RegularWebsockets.Attributes;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RegularWebsockets.Websockets
{
    public static class SocketServiceExtensions
    {
        public static void AddRegularWebSockets(this IServiceCollection services)
        {
            RegisterHandlers(Assembly.GetEntryAssembly().GetTypes());
            RegisterHandlers(Assembly.GetEntryAssembly()
                .GetReferencedAssemblies()
                .Select(a => Assembly.Load(a))
                .SelectMany(a => a.GetTypes()));
        }

        private static void RegisterHandlers(IEnumerable<Type> types)
        {
            types
                .ToList()
                .Where(type => typeof(ISocketService).IsAssignableFrom(type))
                .ToList()
                .ForEach(d =>
                {
                    var pathAttribute = d.GetTypeInfo().GetCustomAttributes<RegularWebsocketsAttribute>();
                    var path = pathAttribute.FirstOrDefault()?.path;

                    if (!string.IsNullOrEmpty(path))
                        SocketHandler.RegisterHandler(path, d);
                });
        }
    }
}
