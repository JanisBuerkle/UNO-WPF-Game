using System;
using System.Windows;
using System.Windows.Input;
using UNO_Spielprojekt.MultiplayerRooms;
using Microsoft.AspNetCore.SignalR.Client;

namespace UNO_Spielprojekt.Window;

public partial class MainWindowView : System.Windows.Window
{
    public static MainWindowView? Instance { get; private set; }

    private HubConnection _hubConnection;

    public MainWindowView()
    {
        InitializeComponent();
        InitializeSignalR();
        Instance = this;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }

    private async void InitializeSignalR()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl("http://localhost:5221/myHub") // Passe die URL entsprechend an
            .Build();
        _hubConnection.On<string>("EmpfangeNachricht", nachricht =>
        {
            // Hier wird die Methode aufgerufen, wenn eine Nachricht empfangen wird
            MessageBox.Show(nachricht);
        });
    }
}