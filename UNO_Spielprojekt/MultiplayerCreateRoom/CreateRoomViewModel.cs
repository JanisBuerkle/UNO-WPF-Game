using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public class CreateRoomViewModel : ViewModelBase
{
    private bool isChecked;

    private bool isEnabled;
    public RelayCommand CloseCreateRoomCommand { get; }
    public RelayCommand GoToGiveNameCommand { get; }
    private MainViewModel MainViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }

    public CreateRoomViewModel(MainViewModel mainViewModel,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        GoToGiveNameCommand = new RelayCommand(GoToGiveNameCommandMethod);
        CloseCreateRoomCommand = new RelayCommand(CloseCreateRommCommandMethod);
    }

    private void GoToGiveNameCommandMethod()
    {
        MainViewModel.CreateRoomVisible = false;
        MainViewModel.GiveNameVisible = true;
    }

    private void CloseCreateRommCommandMethod()
    {
        MainViewModel.CreateRoomVisible = false;
        MainViewModel.MultiplayerRoomsViewModel.DisableAll = true;
    }

    public bool IsChecked
    {
        get => isChecked;
        set
        {
            isChecked = value;
            OnPropertyChanged();
        }
    }

    public bool IsEnabled
    {
        get => isEnabled;
        set
        {
            if (isEnabled != value)
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}