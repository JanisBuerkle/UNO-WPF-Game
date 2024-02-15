using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using WpfApp2.MultiplayerLobby;
using WpfApp2.MultiplayerRooms;
using WpfApp2.Window;

namespace WpfApp2.MultiplayerGiveName;

public class GiveNameViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand CloseGiveNameCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }


    
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
    
    public GiveNameViewModel(MainViewModel mainViewModel, ILogger logger, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        CloseGiveNameCommand = new RelayCommand(CloseGiveNameCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
    }
    
    private void CloseGiveNameCommandMethod()
    {
        MainViewModel.GiveNameVisible = false;
        MultiplayerRoomsViewModel.DisableAll = true;
    }

    private void GoToLobbyCommandMethod()
    {
        MainViewModel.GoToLobby();
        MultiplayerRoomsViewModel.UpdateOnlinePlayer(true);
    }
}