using chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat.Services
{
    public interface IChatRoomService
    {

         Task AddMessage(string RoomNumber, ChatMessage message);



         Task<IEnumerable<ChatMessage>> GetMessageHistory(string RoomNumber);

        Task AddUser(string RoomNumber, string UserName);

        Task RemoveUser(string RoomNumber, string UserName);

        Task<IEnumerable<string>> UserList(string RoomNumber);

    }
}
