using Microsoft.AspNetCore.Http;
using MultiConnectionServer.Services;
using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace MultiConnectionServer.Middlewares
{
     public class ChatWebSocketMiddleware
     {
          private readonly RequestDelegate _next;
          private readonly ChatRoomSocketService _chatRoomSocketService;

          public ChatWebSocketMiddleware(RequestDelegate next)
          {
               _next = next;
               _chatRoomSocketService = new ChatRoomSocketService();
          }

          public async Task Invoke(HttpContext context)
          {
               if (!context.WebSockets.IsWebSocketRequest)
               {
                    await _next.Invoke(context);
                    return;
               }

               CancellationToken ct = context.RequestAborted;
               WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();
               var roomName = context.Request.Path.ToString().Remove(0, 1);
               var socketId = Guid.NewGuid().ToString();

               _chatRoomSocketService.AddSocketToRoom(currentSocket, socketId, roomName);

               while (true)
               {
                    if (ct.IsCancellationRequested) break;

                    var exit = await _chatRoomSocketService.SendMessagesToAllRoomParticipantsAsync(currentSocket, roomName, ct);
                    if (exit) break;
               }

               _chatRoomSocketService.RemoveSocketFromRoom(socketId, roomName);

               await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
               currentSocket.Dispose();
          }
     }
}
