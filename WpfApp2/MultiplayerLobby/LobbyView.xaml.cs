using System.Windows;
using WpfApp2.MultiplayerRooms;

namespace WpfApp2.MultiplayerLobby;

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
}