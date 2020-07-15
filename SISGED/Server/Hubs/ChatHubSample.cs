using Microsoft.AspNetCore.SignalR;
using SISGED.Shared.DTOs;
using SISGED.Shared.Models;
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
            await Groups.AddToGroupAsync(connectionid, user);
            //await Clients.AllExcept(connectionid).SendAsync("SomeoneJoinRoom", user);
        }

        public async Task DisconnectToRoom(string connectionid, string user)
        {
            await Clients.AllExcept(connectionid).SendAsync("SomeoneLeftRoom", user);
        }

        public async Task SendMessageBandeja(string user, ExpedienteBandejaDTO bandeja)
        {
            await Clients.Group(user).SendAsync("ReceiveMessageBandeja", user, bandeja);
        }

        public async Task SendNotification(string user, NotificacionDTO notificacion)
        {
            await Clients.Group(user).SendAsync("ReceiveNotification", user, notificacion);
        }
    }
}
