using tt.Tools.Logging;
using WpfApp2.Window;
using CommunityToolkit.Mvvm.Input;
using WpfApp2.MultiplayerRooms;

namespace WpfApp2.MultiplayerPassword;

public class PasswordViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand ClosePasswordCommand { get; }
    
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

    public string PasswordInput { get; set; }

    public PasswordViewModel(MainViewModel mainViewModel, ILogger logger, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        ClosePasswordCommand = new RelayCommand(ClosePasswordCommandMethod);
    }

    private void ClosePasswordCommandMethod()
    {
        MainViewModel.PasswordVisible = false;
    }
    
    public void PasswordCorrect()
    {
        MainViewModel.PasswordVisible = false;
        MainViewModel.GiveNameVisible = true;
    }
}