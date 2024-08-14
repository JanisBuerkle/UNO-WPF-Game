using System;
using Microsoft.AspNetCore.SignalR.Client;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Service;

public class HubService
{
    private readonly HubConnection hubConnection;
    private readonly MainViewModel mainViewModel;
    private readonly MultiplayerRoomsViewModel multiplayerRoomsViewModel;

    public HubService(MainViewModel mainViewModel, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        this.multiplayerRoomsViewModel = multiplayerRoomsViewModel;
        this.mainViewModel = mainViewModel;
        hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:5000/myHub").Build();
        InitializeSignalR();
    }

    private async void InitializeSignalR()
    {
        hubConnection.On<string>("GetAllRooms", nachricht => { multiplayerRoomsViewModel.GetRooms(); });

        hubConnection.On<string>("ConnectToRoom", nachricht => { mainViewModel.GoToMultiplayerGame(); });

        hubConnection.On<string>("OpenWinnerPage", nachricht =>
        {
            multiplayerRoomsViewModel.RoomClient.RemovePlayer(multiplayerRoomsViewModel.SelectedRoom2, (int)multiplayerRoomsViewModel.Player.Id);
            multiplayerRoomsViewModel.RoomClient.ResetRoom("name", multiplayerRoomsViewModel.SelectedRoom2);

            mainViewModel.WinnerViewModel.IsOnline = true;
            mainViewModel.GoToWinner();
            var splitted = nachricht.Split("-");

            mainViewModel.WinnerViewModel.WinnerName = splitted[0];
            mainViewModel.WinnerViewModel.MoveCounter = splitted[1];
        });

        hubConnection.On<string>("NextPlayersMove", nachricht => { mainViewModel.MultiplayerGamePageViewModel.DiableAllFunctions(); });

        try
        {
            await hubConnection.StartAsync();
            Console.WriteLine(@"Verbindung hergestellt.");
            var signalRUserId = hubConnection.ConnectionId;
            mainViewModel.MultiplayerRoomsViewModel.SignalRId = signalRUserId;
        }
        catch (Exception ex)
        {
            Console.WriteLine(@$"Fehler beim Verbinden: {ex.Message}");
        }
    }
}