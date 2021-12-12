using Microsoft.AspNetCore.Mvc;
using MultiConnectionServer.Services;

namespace MultiConnectionServer.Controllers
{
     [ApiController]
     [Route("[controller]")]
     public class RoomController : ControllerBase
     {
          private ChatRoomSocketService _chatRoomSocketService;

          public RoomController()
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
