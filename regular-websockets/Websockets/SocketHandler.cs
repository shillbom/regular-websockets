using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    GetInstance(http, app).OnOpen(new OpenEvent
                    {
                        Socket = webSocket,
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
                                GetInstance(http, app).OnMessage(new RecieveEvent
                                {
                                    Message = recievedMessage,
                                    MessageType = result.MessageType,
                                    socket = webSocket
                                });
                            }
                        }
                    }

                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                    GetInstance(http, app).OnClose(new CloseEvent
                    {
                        Socket = webSocket,
                        Reason = webSocket.CloseStatus ?? WebSocketCloseStatus.Empty
                    });
                }
                else
                {
                    await next();
                }
            });
        }

        private static ISocketService GetInstance(HttpContext http, IApplicationBuilder app) {
            var handler = LocateHandler(http);
            var instance = ActivatorUtilities.GetServiceOrCreateInstance(http.RequestServices, handler) as ISocketService;
            if (instance == null) {
                instance = ActivatorUtilities.GetServiceOrCreateInstance(app.ApplicationServices, handler) as ISocketService;
            }

            return instance;
        }

        private static Type LocateHandler(HttpContext http)
        {
            return RegisteredHandlers.First(h => http.Request.Path.StartsWithSegments(h.Key)).Value;
        }
    }
}
