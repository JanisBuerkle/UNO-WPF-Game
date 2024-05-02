﻿using System;
using Microsoft.AspNetCore.SignalR.Client;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Service;

public class HubService
{
    private HubConnection _hubConnection;
    private readonly MainViewModel _mainViewModel;
    private readonly MultiplayerRoomsViewModel _multiplayerRoomsViewModel;

    public HubService(MainViewModel mainViewModel, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _multiplayerRoomsViewModel = multiplayerRoomsViewModel;
        _mainViewModel = mainViewModel;
        InitializeSignalR();
    }

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5000/myHub")
            .Build();


        _hubConnection.On<string>("GetAllRooms", nachricht =>
        {
            _multiplayerRoomsViewModel.GetRooms();
        });
        
        _hubConnection.On<string>("ConnectToRoom", nachricht =>
        {
            _mainViewModel.GoToMultiplayerGame();
        });
        
        _hubConnection.On<string>("NextPlayersMove", nachricht =>
        {
            _mainViewModel.MultiplayerGamePageViewModel.DiableAllFunctions();
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