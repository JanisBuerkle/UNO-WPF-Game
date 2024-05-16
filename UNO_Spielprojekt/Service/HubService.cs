using System;
using Microsoft.AspNetCore.SignalR.Client;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using UNO.Contract;

namespace UNO_Spielprojekt.Service;

public class HubService
{
    private readonly HubConnection _hubConnection;
    private readonly MainViewModel _mainViewModel;
    private readonly MultiplayerRoomsViewModel _multiplayerRoomsViewModel;

    public HubService(MainViewModel mainViewModel, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _multiplayerRoomsViewModel = multiplayerRoomsViewModel;
        _mainViewModel = mainViewModel;
        _hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/myHub").Build();
        InitializeSignalR();
    }

    private async void InitializeSignalR()
    {
        _hubConnection.On<string>("GetAllRooms", nachricht =>
        {
            _multiplayerRoomsViewModel.GetRooms();
        });

        _hubConnection.On<string>("ConnectToRoom", nachricht =>
        {
            _mainViewModel.GoToMultiplayerGame();
        });        
        
        _hubConnection.On<string>("OpenWinnerPage", nachricht =>
        {
            _multiplayerRoomsViewModel.RoomClient.RemovePlayer(_multiplayerRoomsViewModel.SelectedRoom2, (int)_multiplayerRoomsViewModel.Player.Id);
            _multiplayerRoomsViewModel.RoomClient.ResetRoom(_multiplayerRoomsViewModel.Player.Name, _multiplayerRoomsViewModel.SelectedRoom2);
            
            _mainViewModel.WinnerViewModel.IsOnline = true;
            _mainViewModel.GoToWinner();
            string[] splitted = nachricht.Split("-");
            
            _mainViewModel.WinnerViewModel.WinnerName = splitted[0];
            _mainViewModel.WinnerViewModel.MoveCounter = splitted[1];
        });

        _hubConnection.On<string>("NextPlayersMove", nachricht =>
        {
            _mainViewModel.MultiplayerGamePageViewModel.DiableAllFunctions();
        });
        
        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine(@"Verbindung hergestellt.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(@$"Fehler beim Verbinden: {ex.Message}");
        }
    }
}