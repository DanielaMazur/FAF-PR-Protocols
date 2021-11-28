using Microsoft.AspNetCore.Mvc;
using MultiConnectionServer.Services;

namespace MultiConnectionServer.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class Room : ControllerBase
     {
          private ChatRoomSocketService _chatRoomSocketService;

          public Room()
          {
               _chatRoomSocketService = new ChatRoomSocketService();
          }

          [HttpPost]
          public void Post(string name)
          {
               _chatRoomSocketService.AddRoom(name);
               return;
          }
     }
}
