using System;
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
            .WithUrl("http://localhost:5221/myHub")
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