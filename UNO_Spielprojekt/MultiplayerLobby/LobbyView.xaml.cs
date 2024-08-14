using System.Windows;

namespace UNO_Spielprojekt.MultiplayerLobby;

public partial class LobbyView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(LobbyViewModel), typeof(LobbyView), new PropertyMetadata(default(LobbyViewModel)));

    public LobbyView()
    {
        InitializeComponent();
    }

    public LobbyViewModel ViewModel
    {
        get => (LobbyViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}