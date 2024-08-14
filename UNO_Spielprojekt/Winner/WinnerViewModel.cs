using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Winner;

public class WinnerViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    private readonly MultiplayerRoomsViewModel _multiplayerRoomsViewModel;
    private string _winnerName;

    private string _moveCounter;

    private bool _isOnline;
    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand BackToTheRoomCommand { get; }

    public WinnerViewModel(MainViewModel mainViewModel, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _mainViewModel = mainViewModel;
        _multiplayerRoomsViewModel = multiplayerRoomsViewModel;
        GoToMainMenuCommand = new RelayCommand(mainViewModel.GoToMainMenu);
        BackToTheRoomCommand = new RelayCommand(BackToTheRoomCommandMethod);
    }

    private async void BackToTheRoomCommandMethod()
    {
        IsOnline = false;

        await _multiplayerRoomsViewModel.RoomClient.AddPlayer(_multiplayerRoomsViewModel.SelectedRoom2, $"{_multiplayerRoomsViewModel.Player.Name}-{_multiplayerRoomsViewModel}");

        _mainViewModel.GoToLobby();
    }

    public string WinnerName
    {
        get => _winnerName;
        set
        {
            if (_winnerName != value)
            {
                _winnerName = value;
                OnPropertyChanged();
            }
        }
    }

    public string MoveCounter
    {
        get => _moveCounter;
        set
        {
            if (_moveCounter != value)
            {
                _moveCounter = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsOnline
    {
        get => _isOnline;
        set
        {
            if (_isOnline != value)
            {
                _isOnline = value;
                OnPropertyChanged();
            }
        }
    }
}