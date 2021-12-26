using System.Threading.Tasks;

namespace MultiConnectionServer
{
     public interface IChatClient
     {
          Task ReceiveMessage(ChatMessage message);
     }
}
