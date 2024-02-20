using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.MultiplayerCreateRoom;
using UNO_Spielprojekt.Service;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly ApiService _apiService;
    public MainViewModel MainViewModel { get; set; }
    public ObservableCollection<Rooms> RoomList { get; set; } = new ObservableCollection<Rooms>();
    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand GoToLobbyCommand { get; }
    public RelayCommand CreateRoomCommand { get; }
    public RelayCommand Testtt { get; }


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
                if (SelectedRoom != null)
                {
                    SelectedRoom2 = SelectedRoom;
                }

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

    public List<Rooms> Rooms { get; set; }
    public HttpClient HttpClient;

    public async Task GetRoom()
    {
        RoomList.Clear();
        HttpClient = new HttpClient();
        var respone = await HttpClient.GetAsync("http://localhost:5221/api/Rooms");
        respone.EnsureSuccessStatusCode();

        var GettedLobbies = await respone.Content.ReadAsStringAsync();

        Rooms = JsonConvert.DeserializeObject<List<Rooms>>(GettedLobbies);
        foreach (var room in Rooms)
        {
            RoomList.Add(room);
        }
    }


    public async Task UpdateOnlinePlayer(bool removeOrAdd)
    {
        HttpClient = new HttpClient();
        var respone = await HttpClient.GetAsync("http://localhost:5221/api/Rooms");
        respone.EnsureSuccessStatusCode();

        var GettedLobbies = await respone.Content.ReadAsStringAsync();

        Rooms = JsonConvert.DeserializeObject<List<Rooms>>(GettedLobbies);

        Rooms roomToUpdate = Rooms.Last();
        HttpClient = new HttpClient();

        var apiUrl = $"http://localhost:5221/api/Rooms/{roomToUpdate.Id}";

        if (removeOrAdd) //add
        {
            roomToUpdate.OnlineUsers += 1;
            roomToUpdate.PlayerNames.Add(new MultiplayerPlayer() { Names = PlayerName });
        }
        else //remove
        {
            roomToUpdate.OnlineUsers -= 1;
            int index = 0;
            bool removeornot = false;
            foreach (var player in roomToUpdate.PlayerNames)
            {
                if (player.Names == PlayerName)
                {
                    index = roomToUpdate.PlayerNames.IndexOf(player);
                    removeornot = true;
                }
            }

            if (removeornot)
            {
                roomToUpdate.PlayerNames.RemoveAt(index);
                removeornot = false;
            }
        }


        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var response = await HttpClient.PutAsync(apiUrl, httpContent);
        response.EnsureSuccessStatusCode();
    }


    public List<Player> Test { get; set; } = new List<Player>();

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger, ApiService apiService)
    {
        GetRoom();
        RoomList.Add(new Rooms()
            { RoomName = "Room1", MaximalUsers = 5, PasswordSecured = true, OnlineUsers = 0, Password = "123" });
        _apiService = apiService;
        _logger = logger;
        MainViewModel = mainViewModel;

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        GoToLobbyCommand = new RelayCommand(GoToLobbyCommandMethod);
        CreateRoomCommand = new RelayCommand(CreateRoomCommandMethod);
        Testtt = new RelayCommand(TestttCommandMethod);
    }

    public void TestttCommandMethod()
    {
        while (true)
        {
            if (SelectedRoom == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine(SelectedRoom.RoomName);
            }

            Thread.Sleep(100);
        }
    }


    private void GoToMainMenuCommandMethod()
    {
        UpdateOnlinePlayer(false);

        _logger.Info("MainMenu wurde geöffnet.");
        MainViewModel.GoToMainMenu();
    }

    private void GoToLobbyCommandMethod()
    {
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