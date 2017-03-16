using Microsoft.AspNetCore.Builder;
using RegularWebsockets.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegularWebsockets.Websockets
{

    public static class SocketHandler
    {
        public static EventHandler<OpenEvent> OnOpen { get; set; }
        public static EventHandler<CloseEvent> OnClose { get; set; }

        public static void UseRegularWebsockets(this IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(async (http, next) =>
            {
                if (http.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                    var extendedSocket = new RegularWebSocket(webSocket);
                    
                    OnOpen(webSocket, new OpenEvent
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

                    OnClose(webSocket, new CloseEvent
                    {
                        Socket = extendedSocket,
                        Reason = webSocket.CloseStatus?? WebSocketCloseStatus.Empty
                    });
                }
                else
                {
                    await next();
                }
            });
        }
    }
}
