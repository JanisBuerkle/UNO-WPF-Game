// In UNO_Server/MeinHub.cs
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

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