using Microsoft.AspNetCore.SignalR;
namespace BabySitting.Api.Shared;

public class ChatHub : Hub
{
    public async Task DirectMessage(string user, string message)
    {
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", user, message);
    }

    public async Task MarkMessageAsRead(string user, string messageId)
    {
        await Clients.User(user).SendAsync("MessageRead", messageId);
    }
}
