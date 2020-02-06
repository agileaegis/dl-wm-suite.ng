using System;
using System.Threading.Tasks;
using dl.wm.suite.interprocess.api.SignalR.Models;
using Microsoft.AspNetCore.SignalR;

namespace dl.wm.suite.interprocess.api.SignalR
{
    public class MessageHub : Hub
    {
        public async void Send(MessageModel message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await Clients.All.SendAsync("Broadcast", message);
        }

        public async Task JoinGroup(string customerId, string deviceId)
        {
            //Todo: customerId Check if Customer with CustomerId can Register to this Group (CustomerId)
            //deviceId = GroupName

            await Groups.AddToGroupAsync(Context.ConnectionId, deviceId);
            await Clients.Group(deviceId).SendAsync("JoinedForGroup", $"Customer with Id: {customerId} joined to Group: {deviceId}");
        }

        public async Task LeaveGroup(string customerId, string deviceId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, deviceId);
            await Clients.Group(deviceId).SendAsync("LeftGroup", $"Customer with Id: {customerId} left Group: {deviceId}");
        }
    }
}