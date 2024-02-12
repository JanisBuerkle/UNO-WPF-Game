using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.MultiplayerCreateRoom;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerRoomsViewModel : ViewModelBase
{
    private readonly ILogger _logger;
    public MainViewModel MainViewModel { get; set; }
    public ObservableCollection<Rooms> Liste { get; set; } = new ObservableCollection<Rooms>();
    public RelayCommand GoToMainMenuCommand { get; }
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

    public MultiplayerRoomsViewModel(MainViewModel mainViewModel, ILogger logger)
    {
        Liste.Add(new Rooms() { RoomName = "Room1000000000", MaximalUsers = 5, PasswordSecured = false});
        Liste.Add(new Rooms() { RoomName = "Room2", MaximalUsers = 2, PasswordSecured = false});
        Liste.Add(new Rooms() { RoomName = "Room3", MaximalUsers = 3, PasswordSecured = true, Password = "123" });
        _logger = logger;
        MainViewModel = mainViewModel;
        
        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        CreateRoomCommand = new RelayCommand(CreateRoomCommandMethod);
    }

    private void GoToMainMenuCommandMethod()
    {
        _logger.Info("MainMenu wurde geöffnet.");
        MainViewModel.GoToMainMenu();
    }

    private void CreateRoomCommandMethod()
    {
        MainViewModel.CreateRoomVisible = true;
        DisableAll = false;
    }
}