using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;
using Wpf.Ui.Common;

namespace UNO_Spielprojekt.MultiplayerLobby;

public class LobbyViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public RelayCommand StartRoomCommand { get; }

    public LobbyViewModel(MainViewModel mainViewModel, ILogger logger,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        StartRoomCommand = new RelayCommand(StartRoomCommandMethod);
    }

    private void StartRoomCommandMethod()
    {
        MultiplayerRoomsViewModel.RoomClient.StartRoom(MultiplayerRoomsViewModel.SelectedRoom2);
        MainViewModel.GoToMultiplayerGame();
    }
}