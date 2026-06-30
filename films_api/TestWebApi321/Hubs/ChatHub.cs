using Microsoft.AspNetCore.SignalR;

namespace TestWebApi321.Hubs;

public class ChatHub : Hub
{
    public async Task JoinAdminGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
    }
    
    public async Task JoinFilmGroup(int filmId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Film_{filmId}");
    }

    public async Task JoinPrivateGroup(int userId, int recipientId)
    {
        var roomId = userId < recipientId ? $"{userId}_{recipientId}" : $"{recipientId}_{userId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Private_{roomId}");
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
    
    public async Task NotifyDelete(int messageId, int? filmId, int? userId, int? recipientId)
    {
        string groupName = GetGroupName(filmId, userId, recipientId);
        await Clients.Group(groupName).SendAsync("MessageDeleted", messageId);
    }
    
    public async Task NotifyUpdate(int messageId, string? newContent, string? imageUrl, int? filmId, int? userId, int? recipientId)
    {
        string groupName = GetGroupName(filmId, userId, recipientId);
        await Clients.Group(groupName).SendAsync("MessageUpdated", messageId, newContent, imageUrl);
    }

    
    private string GetGroupName(int? filmId, int? userId, int? recipientId)
    {
        if (filmId.HasValue) return $"Film_{filmId}";
        var roomId = userId < recipientId ? $"{userId}_{recipientId}" : $"{recipientId}_{userId}";
        return $"Private_{roomId}";
    }

}