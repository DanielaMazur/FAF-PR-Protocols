using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace MultiConnectionServer.Entities
{
     public class Room
     {
          public string Name { get; set; }
          public ConcurrentDictionary<string, WebSocket> Sockets = new();

          public Room(string roomName)
          {
               Name = roomName;
          }
     }
}
