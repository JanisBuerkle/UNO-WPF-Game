using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using UNO_Spielprojekt.Window;
using System.Threading.Tasks;
using tt.Tools.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
using UNO.Contract;
using System.Linq;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    public ObservableCollection<string> ComboBoxItems { get; } = new ObservableCollection<string>
    {
        "2", "3", "4", "5"
    };

    private PlayerDTO _player;
    public PlayerDTO Player
    {
        get => _player;
        set
        {
            if (_player != value)
            {
                _player = value;
                OnPropertyChanged();
            }
        }
    }

    private int _selectedItem;
    public int SelectedMaximalCount
    {
        get => _selectedItem;
        set
        {
            if (_selectedItem != value)
            {
                _selectedItem = value;
                OnPropertyChanged();
            }

            RoomClient.UpdateMaximalPlayers(SelectedRoom2, SelectedMaximalCount);
            GetRooms();
        }
    }

    private ObservableCollection<RoomDTO> _roomList = new ObservableCollection<RoomDTO>();
    public ObservableCollection<RoomDTO> RoomList
    {
        get => _roomList;
        private set
        {
            if (_roomList != value)
            {
                _roomList = value;
                OnPropertyChanged();
            }
        }
    }


    private bool _disableAll;
    public bool DisableAll
    {
        get => _disableAll;
        set
        {
            if (_disableAll != value)
            {
                _disableAll = value;
                OnPropertyChanged();
            }
        }
    }

    public string PlayerName { get; set; }

    private RoomDTO _selectedRoom;
    public RoomDTO SelectedRoom
    {
        get => _selectedRoom;
        set
        {
            if (_selectedRoom != value)
            {
                _selectedRoom = value;

                OnPropertyChanged();
            }
        }
    }

    private RoomDTO _selectedRoom2;
    public RoomDTO SelectedRoom2
    {
        get => _selectedRoom2;
        set
        {
            if (_selectedRoom2 != value)
            {
                _selectedRoom2 = value;
                OnPropertyChanged();
            }
        }
    }

    private List<RoomDTO>? Rooms { get; set; }
    public List<PlayerDTO>? Players { get; set; }
    public HttpClient HttpClient;

    public async Task GetRooms()
    {
        _logger.Info(
            "GetRooms wurde ausgeführt, alle Räume wurden gegettet und entsprechende Propertys wurden gesetzt.");
        Task<string> gettedRoomsTask = RoomClient.GetAllRooms();
        var gettedRooms = await gettedRoomsTask;

        Rooms = JsonConvert.DeserializeObject<List<RoomDTO>>(gettedRooms);

        Application.Current.Dispatcher.InvokeAsync(() =>
        {
            if (Rooms != null) RoomList = new ObservableCollection<RoomDTO>(Rooms);
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
        }

        foreach (var room in Rooms)
        {
            if (room.OnlineUsers == 5)
            {
                room.PlayButtonEnabled = false;
                room.PlayButtonContent = "Voll..";
            }
        }

        foreach (var card in SelectedRoom2.Center)
        {
        }
    }

    public async Task UpdateOnlinePlayer(bool removeOrAdd)
    {
        await GetRooms();
        RoomDTO roomToUpdate = SelectedRoom2;

        HttpClient = new HttpClient();

        if (removeOrAdd) //add
        {
            await RoomClient.AddPlayer(roomToUpdate, PlayerName);
        }
        else //remove
        {
            await RoomClient.RemovePlayer(roomToUpdate, PlayerName, Players);
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

    private readonly ILogger _logger;
    public MainViewModel MainViewModel { get; set; }
    public RoomClient RoomClient { get; set; }
    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }
    public RelayCommand CreateRoomCommand { get; }

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger, RoomClient roomClient)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        RoomClient = roomClient;

        _selectedItem = 5;
        _logger.Info(
            "_selectedItem wurde auf den Standartwert 5 gesetzt, MultiplayerRoomsViewModel Constructor wurde ausgeführt.");

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
        CreateRoomCommand = new RelayCommand(CreateRoomCommandMethod);
    }


    private void GoToMainMenuCommandMethod()
    {
        UpdateOnlinePlayer(false);

        _logger.Info("MainMenu wurde geöffnet, GoToMainMenuCommandMethod wurde ausgeführt.");
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
}