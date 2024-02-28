using Microsoft.AspNetCore.SignalR;

namespace UNO_Server.Hubs
{
    public class MyHub : Hub
    {
        public async Task SendNachricht(string nachricht)
        {
            await Clients.All.SendAsync("EmpfangeNachricht", nachricht);
        }
    }
}