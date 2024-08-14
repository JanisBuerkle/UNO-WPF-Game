using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.SignalR;

namespace UNO_Server.Hubs;

public class MyHub : Hub
{
    public async Task SendGetAllRooms(string nachricht)
    {
        await Clients.All.SendAsync("GetAllRooms", nachricht);
    }

    public async Task NextPlayersMove(string nachricht)
    {
        await Clients.All.SendAsync("NextPlayersMove", nachricht);
    }

    public async Task OpenWinnerPage(string nachricht)
    {
        await Clients.All.SendAsync("OpenWinnerPage", nachricht);
    }

    public async Task ConnectToRoom(string nachricht)
    {
        await Clients.All.SendAsync("ConnectToRoom", nachricht);
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var httpClient = new HttpClient();
        var json = JsonSerializer.Serialize(Context.ConnectionId);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await httpClient.PutAsync($"http://localhost:5000/api/Rooms/removePlayer/{Context.ConnectionId}", content);
        response.EnsureSuccessStatusCode();
    
        await base.OnDisconnectedAsync(exception);
    }
}