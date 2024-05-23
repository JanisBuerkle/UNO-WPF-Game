using UNO_Spielprojekt.MultiplayerRooms;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;

namespace UNO_Spielprojekt.MultiplayerPassword;

public class PasswordViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand ClosePasswordCommand { get; }
    public string PasswordInput { get; set; }

    public PasswordViewModel(MainViewModel mainViewModel, ILogger logger,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
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