using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.MultiplayerCreateRoom;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    private readonly ILogger _logger;
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

    public List<Player> Test { get; set; } = new List<Player>();

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger)
    {
        RoomList.Add(new Rooms() { RoomName = "Room1", MaximalUsers = 5, PasswordSecured = true, OnlineUsers = 0, Password = "123"});

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
        int index = 0;
        bool removeornot = false;
        foreach (var player in SelectedRoom2.PlayerNames)
        {
            if (player.Names == PlayerName)
            {
                index = SelectedRoom2.PlayerNames.IndexOf(player);
                removeornot = true;
            }
        }

        if (removeornot)
        {
            SelectedRoom2.PlayerNames.RemoveAt(index);
            removeornot = false;
        }

        SelectedRoom2.OnlineUsers -= 1;
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
        SelectedRoom2 = SelectedRoom;

        MainViewModel.CreateRoomVisible = true;
    }
}