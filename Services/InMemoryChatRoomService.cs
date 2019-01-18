using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chat.Models;

namespace chat.Services
{
     public class InMemoryChatRoomService : IChatRoomService

    {

        private readonly Dictionary<string, List<ChatMessage>>
       _messageHistory = new Dictionary<string, List<ChatMessage>>();


        private readonly Dictionary<string, List<string>>
        _userList = new Dictionary<string, List<string>>();


        public Task AddMessage(string RoomNumber, ChatMessage message)
        {
            if (!_messageHistory.ContainsKey(RoomNumber))
            {
                _messageHistory[RoomNumber] = new List<ChatMessage>();
            }

            _messageHistory[RoomNumber].Add(message);

            return Task.CompletedTask;
        }

        public Task AddUser(string RoomNumber, string UserName)
        {
            if (!_userList.ContainsKey(RoomNumber))
            {
                _userList[RoomNumber] = new List<string>();
            }

            _userList[RoomNumber].Add(UserName);

            return Task.CompletedTask;
        }

        public Task RemoveUser(string RoomNumber, string UserName)
        {


            _userList[RoomNumber].Remove(UserName);

            return Task.CompletedTask;
        }


        public Task<IEnumerable<ChatMessage>> GetMessageHistory(string RoomNumber)
        {
            _messageHistory.TryGetValue(RoomNumber, out var messages);

            messages = messages ?? new List<ChatMessage>();
            var sortedMessages = messages
                .OrderBy(x => x.SentAt)
                .AsEnumerable();

            return Task.FromResult(sortedMessages);
        }

        public Task<IEnumerable<string>> UserList(string RoomNumber)
        {
            _userList.TryGetValue(RoomNumber, out var users);

            users = users ?? new List<string>();

            var sortedUsers = users.OrderBy(u => u).AsEnumerable();



            return Task.FromResult(sortedUsers);
        }
    }
}
