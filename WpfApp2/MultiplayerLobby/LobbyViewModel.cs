using tt.Tools.Logging;
using WpfApp2.MultiplayerRooms;
using WpfApp2.Window;

namespace WpfApp2.MultiplayerLobby;

public class LobbyViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    public MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public LobbyViewModel(MainViewModel mainViewModel, ILogger logger, MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
    }
}