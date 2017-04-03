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
        public static EventHandler<OpenEvent> OnOpen { get; set; }
        public static EventHandler<CloseEvent> OnClose { get; set; }
        private static Dictionary<PathString, Type> RegisteredHandlers = new Dictionary<PathString, Type>();

        public static void UseRegularWebsockets(this IApplicationBuilder app, WebSocketOptions options)
        {
            app.UseWebSockets(options);
            SetupListeners(app);
        }

        public static void UseRegularWebsockets(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            SetupListeners(app);
        }

        public static void RegisterHandler(string path, Type handler)
        {
            RegisteredHandlers.Add(new PathString(path.ToLower()), handler);
        }

        private static void SetupListeners(IApplicationBuilder app)
        {
            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var handler = FindHandler(http);
                    var instance = Microsoft.Extensions.DependencyInjection.ActivatorUtilities.GetServiceOrCreateInstance(app.ApplicationServices, handler) as ISocketService;

                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    var extendedSocket = new RegularWebSocket(webSocket);

                    instance.OnOpen(new OpenEvent
                    {
                        Socket = extendedSocket,
                        Request = http.Request
                    });

                    var token = CancellationToken.None;
                    var recieveBuffer = new ArraySegment<Byte>(new Byte[4096]);
                    while (webSocket.State == WebSocketState.Open)
                    {
                        using (var buffer = new MemoryStream())
                        {
                            WebSocketReceiveResult result = null;
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

                    instance.OnClose(new CloseEvent
                    {
                        Socket = extendedSocket,
                        Reason = webSocket.CloseStatus ?? WebSocketCloseStatus.Empty
                    });
                }
                else
                {
                    await next();
                }
            });
        }

        private static Type FindHandler(HttpContext http)
        {
            return RegisteredHandlers.First(h => http.Request.Path.StartsWithSegments(h.Key)).Value;
        }
    }
}
