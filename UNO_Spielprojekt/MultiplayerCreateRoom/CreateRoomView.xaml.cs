using System.Windows.Controls;
using System.Windows;
using UNO.Contract;
using System.Linq;

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

        if (ViewModel.IsChecked && PasswordBox.Text.Contains(" ") || ViewModel.IsChecked && PasswordBox.Text == "")
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
        if (ViewModel.IsChecked && PasswordBox.Text.Contains(" ") || ViewModel.IsChecked && PasswordBox.Text == "")
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
        if (ViewModel.IsChecked && PasswordBox.Text.Contains(" ") || ViewModel.IsChecked && PasswordBox.Text == "")
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
        var room = new RoomDto()
        {
            RoomName = Raumname.Text,
            PasswordSecured = ViewModel.IsChecked,
            Password = PasswordBox.Text,
            MaximalUsers = 5
        };
        //
        ViewModel.MultiplayerRoomsViewModel.RoomList.Add(new RoomDto()
        {
            RoomName = Raumname.Text,
            PasswordSecured = ViewModel.IsChecked,
            Password = PasswordBox.Text,
            MaximalUsers = 5
        });
        ViewModel.MultiplayerRoomsViewModel.SelectedRoom2 = ViewModel.MultiplayerRoomsViewModel.RoomList.Last();
        ViewModel.MultiplayerRoomsViewModel.SelectedRoom2 = await ViewModel.MultiplayerRoomsViewModel.RoomClient.PostRoomAsync(room);
    }
}