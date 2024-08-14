using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerGiveName;

public class GiveNameViewModel : ViewModelBase
{
    private readonly ILogger logger;

    private bool _isEnabled;
    public RelayCommand CloseGiveNameCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }
    private MainViewModel MainViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }

    public GiveNameViewModel(MainViewModel mainViewModel, ILogger loggerr,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        logger = loggerr;
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
}