using Microsoft.AspNetCore.SignalR;

namespace UNO_Server.Hubs
{
    public class MyHub : Hub
    {
        public async Task SendGetAllRooms(string nachricht)
        {
            await Clients.All.SendAsync("GetAllRooms", nachricht);
        }
        
        public async Task SendGetRoom(int roomId)
        {
            await Clients.All.SendAsync("GetRoom", roomId);
        }
    }
}