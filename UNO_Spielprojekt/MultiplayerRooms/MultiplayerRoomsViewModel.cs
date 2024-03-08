using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using tt.Tools.Logging;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    public ObservableCollection<string> ComboBoxItems { get; } = new ObservableCollection<string>
    {
        "2", "3", "4", "5"
    };

    private MultiplayerPlayer _player;

    public MultiplayerPlayer Player
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

            UpdateMaximalPlayers(SelectedRoom2);
        }
    }

    private readonly ILogger _logger;
    public MainViewModel MainViewModel { get; set; }

    private ObservableCollection<Rooms> _roomList = new ObservableCollection<Rooms>();

    public ObservableCollection<Rooms> RoomList
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

    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }
    public RelayCommand CreateRoomCommand { get; }


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

    private Rooms _selectedRoom;

    public Rooms SelectedRoom
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

    private Rooms _selectedRoom2;

    public Rooms SelectedRoom2
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

    private List<Rooms>? Rooms { get; set; }
    private List<MultiplayerPlayer>? Players { get; set; }
    private HttpClient _httpClient;
    private bool _playButtonEnabled;

    public async Task GetAllRooms()
    {
        _httpClient = new HttpClient();
        var respone = await _httpClient.GetAsync("http://localhost:5000/api/Rooms");
        respone.EnsureSuccessStatusCode();

        var gettedLobbies = await respone.Content.ReadAsStringAsync();

        Rooms = JsonConvert.DeserializeObject<List<Rooms>>(gettedLobbies);

        Application.Current.Dispatcher.InvokeAsync(() => { RoomList = new ObservableCollection<Rooms>(Rooms); });

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
    }

    public async Task GetPlayers()
    {
        _httpClient = new HttpClient();
        var respone = await _httpClient.GetAsync("http://localhost:5000/api/Player");
        respone.EnsureSuccessStatusCode();

        var gettedLobbies = await respone.Content.ReadAsStringAsync();

        Players = JsonConvert.DeserializeObject<List<MultiplayerPlayer>>(gettedLobbies);
    }

    private async Task AddPlayer(Rooms roomToUpdate)
    {
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/addPlayer/{PlayerName}";

        var response = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    public async void StartRoom()
    {
        var jsonContent = JsonConvert.SerializeObject(SelectedRoom2);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/startroom/{SelectedRoom2.Id}";

        var response = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateMaximalPlayers(Rooms roomToUpdate)
    {
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/updatemaximalplayers/{SelectedMaximalCount}";

        var response = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();
        await GetAllRooms();
    }

    private async Task RemovePlayer(Rooms roomToUpdate)
    {
        await GetPlayers();

        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/removePlayer/{PlayerName}";

        var response2 = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response2.EnsureSuccessStatusCode();


        int id = 0;

        foreach (var player in Players)
        {
            if (player.Name == PlayerName)
            {
                id = (int)player.Id;
            }
        }

        var removePlayerUrl = $"http://localhost:5000/api/Player/{id}";

        var response = await _httpClient.DeleteAsync(removePlayerUrl);
        response.EnsureSuccessStatusCode();


        await GetAllRooms();
    }

    public async Task UpdateOnlinePlayer(bool removeOrAdd)
    {
        await GetAllRooms();
        Rooms roomToUpdate = SelectedRoom2;

        _httpClient = new HttpClient();

        if (removeOrAdd) //add
        {
            await AddPlayer(roomToUpdate);
        }
        else //remove
        {
            await RemovePlayer(roomToUpdate);
        }

        await GetAllRooms();

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

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger)
    {
        GetAllRooms();
        _logger = logger;
        MainViewModel = mainViewModel;

        _selectedItem = 5;

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
        CreateRoomCommand = new RelayCommand(CreateRoomCommandMethod);
    }

    private void GoToMainMenuCommandMethod()
    {
        UpdateOnlinePlayer(false);

        _logger.Info("MainMenu wurde geöffnet.");
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