using MultiConnectionServer.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiConnectionServer.Services
{
     public class ChatRoomSocketService
     {
          private static readonly List<Room> _rooms = new();

          public void AddRoom(string roomName)
          {
               if (_rooms.Exists(room => room.Name == roomName)) return;
               _rooms.Add(new Room(roomName));
          }

          public void RemoveRoom(string roomName)
          {
               _rooms.Remove(_rooms.Find(room => room.Name == roomName));
          }

          public void RemoveSocketFromRoom(string socketId, string roomName)
          {
               var room = _rooms.Find(room => room.Name == roomName);
               room?.Sockets?.TryRemove(socketId, out _);
          }

          public void AddSocketToRoom(WebSocket socket, string socketId, string roomName)
          {
               var room = _rooms.Find(room => room.Name == roomName);
               if (room == null) return;
               room.Sockets.TryAdd(socketId, socket);
          }

          public async Task<bool> SendMessagesToAllRoomParticipantsAsync(WebSocket socket, string roomName, CancellationToken ct)
          {
               var response = await ReceiveStringAsync(socket, ct);
               var room = _rooms.Find(room => room?.Name == roomName);
               if (string.IsNullOrEmpty(response) || room == null)
               {
                    if (socket.State != WebSocketState.Open) return true;
                    return false;
               }
               foreach (var roomSocket in room.Sockets)
               {
                    if (roomSocket.Value.State != WebSocketState.Open) continue;
                    await SendStringAsync(roomSocket.Value, response, ct);
               }
               return false;
          }

          private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default)
          {
               var buffer = Encoding.UTF8.GetBytes(data);
               var segment = new ArraySegment<byte>(buffer);
               return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
          }

          private static async Task<string> ReceiveStringAsync(WebSocket socket, CancellationToken ct = default)
          {
               var buffer = new ArraySegment<byte>(new byte[8192]);
               using var ms = new MemoryStream();
               WebSocketReceiveResult result;
               do
               {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
               }
               while (!result.EndOfMessage);

               ms.Seek(0, SeekOrigin.Begin);
               if (result.MessageType != WebSocketMessageType.Text) return null;

               using var reader = new StreamReader(ms, Encoding.UTF8);
               return await reader.ReadToEndAsync();
          }

     }
}
