using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Service;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public class CreateRoomViewModel : ViewModelBase
{
    private bool _isChecked;
    private readonly ILogger _logger;
    public ApiService _apiService;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand CloseCreateRoomCommand { get; }
    public RelayCommand GoToGiveNameCommand { get; }

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

    private Rooms _createdRoom;

    public Rooms CreatedRoom
    {
        get => _createdRoom;
        set
        {
            if (_createdRoom != value)
            {
                _createdRoom = value;
                OnPropertyChanged();
            }
        }
    }
    

    public CreateRoomViewModel(MainViewModel mainViewModel, ILogger logger, MultiplayerRoomsViewModel multiplayerRoomsViewModel, ApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
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