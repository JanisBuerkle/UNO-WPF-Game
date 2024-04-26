using System.Windows;
using System.Windows.Controls;

namespace UNO_Spielprojekt.MultiplayerRooms;

public partial class MultiplayerRoomsView : UserControl
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(MultiplayerRoomsViewModel), typeof(MultiplayerRoomsView),
        new PropertyMetadata(default(MultiplayerRoomsViewModel)));

    public MultiplayerRoomsViewModel ViewModel
    {
        get => (MultiplayerRoomsViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public MultiplayerRoomsView()
    {
        InitializeComponent();
    }
}