using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UNO_Spielprojekt.MultiplayerRooms;
using Microsoft.AspNetCore.SignalR.Client;

namespace UNO_Spielprojekt.Window;

public partial class MainWindowView : System.Windows.Window
{
    public static MainWindowView? Instance { get; private set; }

    public MainWindowView()
    {
        InitializeComponent();
        Instance = this;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}