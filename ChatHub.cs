using chat.Models;
using chat.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace chat
{
    public class ChatHub : Hub
    {


        private readonly IChatRoomService _chatRoomService;


        public ChatHub(IChatRoomService chatRoomService)

        {
            _chatRoomService = chatRoomService;

        }

        public override async Task OnConnectedAsync()
        {


            var httpContext = Context.GetHttpContext();

            var roomNumber = httpContext.Request.Query["roomNumber"];

            var chatter = httpContext.Request.Query["chatterName"];


            

             var history = await _chatRoomService
            .GetMessageHistory(roomNumber);

            await Clients.Caller.SendAsync(
                "ReceiveMessages", history);
            await AddToGroup(roomNumber, chatter);
            await base.OnConnectedAsync();
        }





        public async Task AddToGroup(string roomNumber, string name)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomNumber);

            var tempUser = await _chatRoomService.UserList(roomNumber);
            if (tempUser.Contains(name))
            {
                var r = new Random();
                name = name + r.Next(10, 100).ToString();
                await Clients.Client(Context.ConnectionId).SendAsync("NewName", name);
            }


            await _chatRoomService.AddUser(roomNumber, name);



            await Clients.Group(roomNumber).SendAsync("Send", $"{name} has joined the group.");

            var users = await _chatRoomService.UserList(roomNumber);

            await Clients.Group(roomNumber).SendAsync(
                   "UserJoin", users);

        }



        public async Task SendMessage(string name, string text,string RoomName)

        {



            var message = new ChatMessage
            {
                SenderName = name,
                Text = text,
                SentAt = DateTimeOffset.UtcNow
            };


            await _chatRoomService.AddMessage(RoomName, message);




            await Clients.Group(RoomName).SendAsync(
                "ReceiveMessage",
                message.SenderName,
                message.SentAt,
                message.Text);
        }

        public async Task RemoveUser(string roomNumber, string name)
        {
            await _chatRoomService.RemoveUser(roomNumber, name);
            await Clients.Group(roomNumber).SendAsync("Left", $"{name} has left the group.");
            var users = await _chatRoomService.UserList(roomNumber);

            await Clients.Group(roomNumber).SendAsync(
                   "UserJoin", users);

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomNumber);


        }






    }
}
