using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.Service;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    private readonly ApiService _apiService;
    public MainViewModel MainViewModel { get; set; }

    private ObservableCollection<Rooms> _roomList = new ObservableCollection<Rooms>();

    public ObservableCollection<Rooms> RoomList
    {
        get => _roomList;
        set
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

    public async Task GetAllRooms()
    {
        HttpClient = new HttpClient();
        var respone = await HttpClient.GetAsync("http://localhost:5221/api/Rooms");
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
        }
    }

    // public async Task GetRoom(int id)
    // {
    //     HttpClient = new HttpClient();
    //     var respone = await HttpClient.GetAsync($"http://localhost:5221/api/Rooms/{id}");
    //     respone.EnsureSuccessStatusCode();
    //
    //     var gettedLobbies = await respone.Content.ReadAsStringAsync();
    //
    //     Rooms = JsonConvert.DeserializeObject<List<Rooms>>(gettedLobbies);
    //
    //     Application.Current.Dispatcher.InvokeAsync(() => { RoomList = new ObservableCollection<Rooms>(Rooms); });
    // }


    public async Task UpdateOnlinePlayer(bool removeOrAdd)
    {
        HttpClient = new HttpClient();
        var respone = await HttpClient.GetAsync("http://localhost:5221/api/Rooms");
        respone.EnsureSuccessStatusCode();

        var GettedLobbies = await respone.Content.ReadAsStringAsync();

        Rooms = JsonConvert.DeserializeObject<List<Rooms>>(GettedLobbies);


        foreach (var room in Rooms)
        {
            if (SelectedRoom2.Id == room.Id)
            {
                SelectedRoom2 = room;
            }
        }

        Rooms roomToUpdate = SelectedRoom2;

        HttpClient = new HttpClient();

        var apiUrl = $"http://localhost:5221/api/Rooms/{roomToUpdate.Id}";

        // ToDo: move delete logic to controller
        if (removeOrAdd) //add
        {
            var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var addPlayerUrl = $"http://localhost:5221/api/Rooms/addPlayer/{PlayerName}";

            var response = await HttpClient.PutAsync(addPlayerUrl, httpContent);
            response.EnsureSuccessStatusCode();
        }
        else //remove
        {
            var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var addPlayerUrl = $"http://localhost:5221/api/Rooms/removePlayer/{PlayerName}";

            var response = await HttpClient.PutAsync(addPlayerUrl, httpContent);
            response.EnsureSuccessStatusCode();


            // roomToUpdate.OnlineUsers -= 1;
            // int index = 0;
            // bool removeornot = false;
            // foreach (var player in roomToUpdate.PlayerNames)
            // {
            //     if (player.Name == PlayerName)
            //     {
            //         index = roomToUpdate.PlayerNames.IndexOf(player);
            //         removeornot = true;
            //     }
            // }
            //
            // if (removeornot)
            // {
            //     roomToUpdate.PlayerNames.RemoveAt(index);
            //     removeornot = false;
            // }
            //
            // var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
            // var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            //
            // var response = await HttpClient.PutAsync(apiUrl, httpContent);
            // response.EnsureSuccessStatusCode();
        }

        await GetAllRooms();
        foreach (var room in Rooms)
        {
            if (SelectedRoom2.Id == room.Id)
            {
                SelectedRoom2 = room;
            }
        }
    }


    public List<Player> Test { get; set; } = new List<Player>();

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger, ApiService apiService)
    {
        GetAllRooms();
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