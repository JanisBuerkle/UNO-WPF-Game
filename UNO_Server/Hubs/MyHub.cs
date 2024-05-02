using Microsoft.AspNetCore.SignalR;

namespace UNO_Server.Hubs
{
    public class MyHub : Hub
    {
        public async Task SendGetAllRooms(string nachricht)
        {
            await Clients.All.SendAsync("GetAllRooms", nachricht);
        }
        
        public async Task OpenChooseColorWindow(string nachricht)
        {
            await Clients.All.SendAsync("OpenChooseColorWidnow", nachricht);
        }
        
        public async Task NextPlayersMove(string nachricht)
        {
            await Clients.All.SendAsync("NextPlayersMove", nachricht);
        }
        
        public async Task ConnectToRoom(string nachricht)
        {
            await Clients.All.SendAsync("ConnectToRoom", nachricht);
        }
    }
}