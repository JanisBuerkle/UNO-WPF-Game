using System.Collections.Generic;
using System.Collections.ObjectModel;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class Rooms : ViewModelBase
{
    public string RoomName { get; set; }
    
    private int _onlineUsers;
    public int OnlineUsers
    {
        get => _onlineUsers;
        set
        {
            if (_onlineUsers != value)
            {
                _onlineUsers = value;
                OnPropertyChanged();
            }
        }
    }
    public int MaximalUsers { get; set;}
    public bool PasswordSecured { get; set;}
    public string Password { get; set;}
    public ObservableCollection<MultiplayerPlayer> PlayerNames { get; set; } = new ObservableCollection<MultiplayerPlayer>();
}