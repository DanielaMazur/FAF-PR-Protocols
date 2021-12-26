using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MultiConnectionServer.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class SignalRController : ControllerBase

     {
          private readonly IHubContext<ChatHub, IChatClient> _chatHub;

          public SignalRController(IHubContext<ChatHub, IChatClient> chatHub)
          {
               _chatHub = chatHub;
          }

          [HttpPost("messages")]
          public async Task Post(ChatMessage message)
          {
               await _chatHub.Clients.All.ReceiveMessage(message);
          }

     }
}
