using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using Wpf.Ui.Common;

namespace UNO_Spielprojekt.MultiplayerLobby;

public class LobbyViewModel : ViewModelBase
{
    private readonly ILogger logger;
    public RelayCommand StartRoomCommand { get; }
    private MainViewModel MainViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }

    public LobbyViewModel(MainViewModel mainViewModel, ILogger loggerr,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        logger = loggerr;
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