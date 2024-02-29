using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class Rooms : ViewModelBase
{
    [JsonProperty("Id")] public long Id { get; set; }
    [JsonProperty("RoomName")] public string RoomName { get; set; }

    [JsonProperty("_onlineUsers")] private int _onlineUsers;

    [JsonProperty("OnlineUsers")]
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

    [JsonProperty("MaximalUsers")] public int MaximalUsers { get; set; }
    [JsonProperty("PasswordSecured")] public bool PasswordSecured { get; set; }
    [JsonProperty("Password")] public string Password { get; set; }


    [JsonProperty("_players")]
    private ObservableCollection<MultiplayerPlayer> _players = new ObservableCollection<MultiplayerPlayer>();

    [JsonProperty("Players")]
    public ObservableCollection<MultiplayerPlayer> Players
    {
        get => _players;
        set
        {
            if (_players != value)
            {
                _players = value;
                OnPropertyChanged();
            }
        }
    }
}