using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UNO_Spielprojekt.MultiplayerRooms;

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

    private void TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Raumname.Text == "" || Raumname.Text == " ")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }

        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private void CheckBoxClicked(object sender, RoutedEventArgs e)
    {
        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private void PasswortBoxChanged(object sender, TextChangedEventArgs e)
    {
        if (ViewModel.IsChecked && PasswortBox.Text.Contains(" ") || ViewModel.IsChecked && PasswortBox.Text == "")
        {
            ViewModel.IsEnabled = false;
        }
        else
        {
            ViewModel.IsEnabled = true;
        }
    }

    private async void CreateRoom(object sender, RoutedEventArgs e)
    {
        var room = new Rooms()
        {
            RoomName = Raumname.Text,
            PasswordSecured = ViewModel.IsChecked,
            Password = PasswortBox.Text,
            MaximalUsers = 5
        };
        await ViewModel._apiService.PostRoomAsync(room);
        
        ViewModel.MultiplayerRoomsViewModel.RoomList.Add(new Rooms()
        {
            RoomName = Raumname.Text,
            PasswordSecured = ViewModel.IsChecked,
            Password = PasswortBox.Text,
            MaximalUsers = 5
        });
        ViewModel.MultiplayerRoomsViewModel.SelectedRoom2 = ViewModel.MultiplayerRoomsViewModel.RoomList.Last();
    }
}