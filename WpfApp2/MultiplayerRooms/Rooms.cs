using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace WpfApp2.MultiplayerRooms;

public class Rooms : ViewModelBase
{
    [JsonProperty("Id")]
    public long Id { get; set; }
    [JsonProperty("RoomName")]
    public string RoomName { get; set; }
    
    [JsonProperty("_onlineUsers")]
    private int _onlineUsers;
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
    [JsonProperty("MaximalUsers")]
    public int MaximalUsers { get; set;}
    [JsonProperty("PasswordSecured")]
    public bool PasswordSecured { get; set;}
    [JsonProperty("Password")]
    public string Password { get; set;}


    [JsonProperty("_playerNames")]
    private ObservableCollection<MultiplayerPlayer> _playerNames = new ObservableCollection<MultiplayerPlayer>();
    [JsonProperty("PlayerNames")]
    public ObservableCollection<MultiplayerPlayer> PlayerNames
    {
        get => _playerNames;
        set
        {
            if (_playerNames != value)
            {
                _playerNames = value;
                OnPropertyChanged();
            }
        }
    }
}