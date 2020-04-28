using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISGED.Server.Hubs
{
    public class ChatHubSample : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);        
        }

        public async Task ConnectToRoom(string connectionid,string user)
        {
            await Clients.AllExcept(connectionid).SendAsync("SomeoneJoinRoom", user);
        }

        public async Task DisconnectToRoom(string connectionid, string user)
        {
            await Clients.AllExcept(connectionid).SendAsync("SomeoneLeftRoom", user);
        }
    }
}
