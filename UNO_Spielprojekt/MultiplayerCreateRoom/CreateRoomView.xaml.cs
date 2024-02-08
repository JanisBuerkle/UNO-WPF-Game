using System.Windows;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public partial class CreateRoomView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel), typeof(CreateRoomViewModel), typeof(CreateRoomView),
        new PropertyMetadata(default(CreateRoomViewModel)));

    public CreateRoomViewModel ViewModel
    {
        get => (CreateRoomViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }


    public CreateRoomView()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        throw new System.NotImplementedException();
    }
}