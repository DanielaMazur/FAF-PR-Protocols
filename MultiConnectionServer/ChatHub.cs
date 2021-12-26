using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MultiConnectionServer
{
     public class ChatHub : Hub<IChatClient>
     {
          public async Task SendMessage(ChatMessage message)
          {
               await Clients.All.ReceiveMessage(message);
          }
     }
}
