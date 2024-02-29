using System.Windows;
using UNO_Spielprojekt.MultiplayerRooms;

namespace UNO_Spielprojekt.MultiplayerLobby;

public partial class LobbyView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(LobbyViewModel), typeof(LobbyView), new PropertyMetadata(default(LobbyViewModel)));

    public LobbyViewModel ViewModel
    {
        get => (LobbyViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    
    public LobbyView()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.MultiplayerRoomsViewModel.SelectedRoom2.Players.Add(new MultiplayerPlayer() {Name = "TestSpieler123", RoomId = 1});
    }
}