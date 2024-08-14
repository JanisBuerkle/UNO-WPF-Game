using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerPassword;

public class PasswordViewModel : ViewModelBase
{
    private readonly ILogger logger;
    public RelayCommand ClosePasswordCommand { get; }
    private MainViewModel MainViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public string PasswordInput { get; set; }

    public PasswordViewModel(MainViewModel mainViewModel, ILogger loggerr,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        logger = loggerr;
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        ClosePasswordCommand = new RelayCommand(ClosePasswordCommandMethod);
    }

    public void PasswordCorrect()
    {
        MainViewModel.PasswordVisible = false;
        MainViewModel.GiveNameVisible = true;
    }

    private void ClosePasswordCommandMethod()
    {
        MainViewModel.PasswordVisible = false;
    }
}