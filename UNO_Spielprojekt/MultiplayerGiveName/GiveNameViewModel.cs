using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerGiveName;

public class GiveNameViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
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
    
    public GiveNameViewModel(MainViewModel mainViewModel, ILogger logger)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        CloseGiveNameCommand = new RelayCommand(CloseGiveNameCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
    }
    
    private void CloseGiveNameCommandMethod()
    {
        MainViewModel.GiveNameVisible = false;
        MainViewModel.MultiplayerRoomsViewModel.DisableAll = true;
    }

    private void GoToLobbyCommandMethod()
    {
        MainViewModel.GoToLobby();
    }
}