using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public class CreateRoomViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand CloseCreateRoomCommand { get; }
    public RelayCommand GoToGiveNameCommand { get; }

    private bool _isChecked;

    public bool IsChecked
    {
        get { return _isChecked; }
        set
        {
            _isChecked = value;
            OnPropertyChanged("IsChecked");
        }
    }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    public CreateRoomViewModel(MainViewModel mainViewModel, ILogger logger,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _logger = logger;
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
}