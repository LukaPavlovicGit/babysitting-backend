﻿using Microsoft.AspNetCore.SignalR;
namespace BabySitting.Api.Shared;

public class OneOnOneChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);
    }
}
