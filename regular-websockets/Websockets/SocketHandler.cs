using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RegularWebsockets.Events;
using RegularWebsockets.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace RegularWebsockets.Websockets
{
    public static class SocketHandler
    {
        private static Dictionary<PathString, Type> RegisteredHandlers = new Dictionary<PathString, Type>();
        private const int bufSize = 1024 * 4;

        public static void UseRegularWebsockets(this IApplicationBuilder app)
        {
            var defaults =

            app.UseWebSockets(new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = bufSize
            });

            SetupListeners(app);
        }

        public static void RegisterHandler(string path, Type handler)
        {
            if (!RegisteredHandlers.ContainsKey(path))
                RegisteredHandlers.Add(new PathString(path.ToLower()), handler);
        }

        private static void SetupListeners(IApplicationBuilder app)
        {
            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var handler = LocateHandler(http);
                    var instance = Microsoft.Extensions.DependencyInjection.ActivatorUtilities.GetServiceOrCreateInstance(app.ApplicationServices, handler) as ISocketService;

                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    var extendedSocket = new RegularWebSocket(webSocket);

                    instance.OnOpen(new OpenEvent
                    {
                        Socket = extendedSocket,
                        Request = http.Request
                    });

                    var recieveBuffer = new ArraySegment<Byte>(new Byte[bufSize]);
                    WebSocketReceiveResult result = null;

                    while (webSocket.State == WebSocketState.Open)
                    {
                        using (var buffer = new MemoryStream())
                        {
                            do
                            {
                                result = await webSocket.ReceiveAsync(recieveBuffer, CancellationToken.None);
                                buffer.Write(recieveBuffer.Array, recieveBuffer.Offset, result.Count);
                            }
                            while (!result.EndOfMessage);

                            buffer.Position = 0;
                            using (var reader = new StreamReader(buffer, Encoding.UTF8))
                            {
                                var recievedMessage = await reader.ReadToEndAsync();
                                extendedSocket.NotifyRecieved(new RecieveEvent
                                {
                                    Message = recievedMessage,
                                    MessageType = result.MessageType
                                });
                            }
                        }
                    }

                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

                    var closed = new CloseEvent
                    {
                        Socket = extendedSocket,
                        Reason = webSocket.CloseStatus ?? WebSocketCloseStatus.Empty
                    };

                    extendedSocket.NotifyClosed(closed);
                    instance.OnClose(closed);
                }
                else
                {
                    await next();
                }
            });
        }

        private static Type LocateHandler(HttpContext http)
        {
            return RegisteredHandlers.First(h => http.Request.Path.StartsWithSegments(h.Key)).Value;
        }
    }
}
