using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using tt.Tools.Logging;
using UNO_Spielprojekt.Window;
using UNO.Contract;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    private readonly ILogger logger;

    private PlayerDto player;

    private int selectedItem;

    private ObservableCollection<RoomDto> roomList = new();


    private bool disableAll;

    private RoomDto selectedRoom;

    private RoomDto selectedRoom2;

    public ObservableCollection<string> ComboBoxItems { get; } = new()
    {
        "2", "3", "4", "5"
    };

    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand LeaveRoomCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }
    public RelayCommand CreateRoomCommand { get; }
    public string SignalRId { get; set; }

    public string PlayerName { get; set; }

    private List<RoomDto>? Rooms { get; set; }
    public List<PlayerDto>? Players { get; set; }
    public MainViewModel MainViewModel { get; set; }
    public IRoomClient RoomClient { get; set; }

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger loggerr, IRoomClient roomClient)
    {
        logger = loggerr;
        MainViewModel = mainViewModel;
        RoomClient = roomClient;

        selectedItem = 5;
        logger.Info(
            "_selectedItem wurde auf den Standartwert 5 gesetzt, MultiplayerRoomsViewModel Constructor wurde ausgeführt.");

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        LeaveRoomCommand = new RelayCommand(LeaveRoomCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
        CreateRoomCommand = new RelayCommand(CreateRoomCommandMethod);
    }

    public async Task GetRooms()
    {
        logger.Info(
            "GetRooms wurde ausgeführt, alle Räume wurden gegettet und entsprechende Propertys wurden gesetzt.");
        var gettedRoomsTask = RoomClient.GetAllRooms();
        var gettedRooms = await gettedRoomsTask;

        Rooms = JsonConvert.DeserializeObject<List<RoomDto>>(gettedRooms);

        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (Rooms != null)
            {
                RoomList = new ObservableCollection<RoomDto>(Rooms);
            }
        });

        if (SelectedRoom2 != null)
        {
            foreach (var room in Rooms)
            {
                if (SelectedRoom2.Id == room.Id)
                {
                    SelectedRoom2 = room;
                }
            }

            foreach (var player in SelectedRoom2.Players)
            {
                if (PlayerName == player.Name)
                {
                    Player = player;

                    Application.Current.Dispatcher.InvokeAsync(() =>
                        MainViewModel.MultiplayerGamePageViewModel.SetCurrentHand());
                }
            }

            MainViewModel.MultiplayerGamePageViewModel.MoveCounter = SelectedRoom2.MoveCounter;
            MainViewModel.MultiplayerGamePageViewModel.RoundCounterString =
                $"Runde: {SelectedRoom2.MoveCounter}/\u221e";

            if (Player != null)
            {
                MainViewModel.MultiplayerGamePageViewModel.DisableAllFunctions =
                    Player.Id == SelectedRoom2.PlayerTurnId;
            }
        }

        foreach (var room in Rooms)
        {
            if (room.OnlineUsers == room.MaximalUsers)
            {
                room.PlayButtonEnabled = false;
                room.PlayButtonContent = "Voll..";
            }
        }
    }

    public async Task UpdateOnlinePlayer(bool removeOrAdd)
    {
        await GetRooms();
        var roomToUpdate = SelectedRoom2;

        if (removeOrAdd) //add
        {
            await RoomClient.AddPlayer(roomToUpdate, $"{PlayerName}-{SignalRId}");
        }
        else //remove
        {
            await RoomClient.RemovePlayer(roomToUpdate, (int)Player.Id);
        }

        await GetRooms();

        foreach (var room in Rooms)
        {
            if (SelectedRoom2.Id == room.Id)
            {
                SelectedRoom2 = room;
            }
        }

        if (removeOrAdd)
        {
            Player = SelectedRoom2.Players.FirstOrDefault(player => player.Name == PlayerName);
        }
    }

    private async void RemoveMorePlayers()
    {
        await RoomClient.RemovePlayer(SelectedRoom2, (int)SelectedRoom2.Players.Last().Id);
    }


    private void GoToMainMenuCommandMethod()
    {
        logger.Info("MainMenu wurde geöffnet, GoToMainMenuCommandMethod wurde ausgeführt.");
        MainViewModel.GoToMainMenu();
    }

    private async void LeaveRoomCommandMethod()
    {
        await UpdateOnlinePlayer(false);
        logger.Info("MainMenu wurde geöffnet, GoToMainMenuCommandMethod wurde ausgeführt.");
        MainViewModel.GoToMainMenu();
    }

    private void GoToLobbyCommandMethod()
    {
        SelectedRoom2 = SelectedRoom;
        if (SelectedRoom2.PasswordSecured)
        {
            MainViewModel.PasswordVisible = true;
        }
        else
        {
            MainViewModel.GiveNameVisible = true;
        }
    }

    private void CreateRoomCommandMethod()
    {
        MainViewModel.CreateRoomVisible = true;
    }

    public PlayerDto Player
    {
        get => player;
        set
        {
            if (player != value)
            {
                player = value;
                OnPropertyChanged();
            }
        }
    }

    public int SelectedMaximalCount
    {
        get => selectedItem;
        set
        {
            if (selectedItem != value)
            {
                selectedItem = value;
                if (value < SelectedRoom2.OnlineUsers)
                {
                    for (var i = SelectedRoom2.OnlineUsers; i > value; i--)
                    {
                        RemoveMorePlayers();
                    }
                }

                OnPropertyChanged();
            }

            RoomClient.UpdateMaximalPlayers(SelectedRoom2, SelectedMaximalCount);
            GetRooms();
        }
    }

    public ObservableCollection<RoomDto> RoomList
    {
        get => roomList;
        private set
        {
            if (roomList != value)
            {
                roomList = value;
                OnPropertyChanged();
            }
        }
    }

    public bool DisableAll
    {
        get => disableAll;
        set
        {
            if (disableAll != value)
            {
                disableAll = value;
                OnPropertyChanged();
            }
        }
    }

    public RoomDto SelectedRoom
    {
        get => selectedRoom;
        set
        {
            if (selectedRoom != value)
            {
                selectedRoom = value;

                OnPropertyChanged();
            }
        }
    }

    public RoomDto SelectedRoom2
    {
        get => selectedRoom2;
        set
        {
            if (selectedRoom2 != value)
            {
                selectedRoom2 = value;
                OnPropertyChanged();
            }
        }
    }
}