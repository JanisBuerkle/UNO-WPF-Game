using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UNO_Spielprojekt.MultiplayerRooms;
using Microsoft.AspNetCore.SignalR.Client;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Service;

public class HubService
{
    private HubConnection _hubConnection;
    private readonly MainViewModel _mainViewModel;
    public HubService(MainViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        InitializeSignalR();
    }
    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5221/myHub") // Passe die URL entsprechend an
            .Build();
        
        
        _hubConnection.On<string>("EmpfangeNachricht", nachricht =>
        {
            _mainViewModel.MultiplayerRoomsViewModel.GetRoom();
        });

        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("Verbindung hergestellt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Verbinden: {ex.Message}");
        }
    }
}