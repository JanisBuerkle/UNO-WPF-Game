using System.Collections.ObjectModel;
using UNO_Spielprojekt.GamePage;
using Newtonsoft.Json;
using UNO.Contract;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class Rooms : ViewModelBase
{
    [JsonProperty("Id")] public long Id { get; set; }
    [JsonProperty("RoomName")] public string RoomName { get; set; }
    [JsonProperty("PlayButtonEnabled")] public bool PlayButtonEnabled { get; set; } = true;

    private string _playButtonContent = "Play";

    [JsonProperty("PlayButtonContent")]
    public string PlayButtonContent
    {
        get => _playButtonContent;
        set
        {
            if (_playButtonContent != value)
            {
                _playButtonContent = value;
                OnPropertyChanged();
            }
        }
    }

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

    private int _maximalUsers;

    [JsonProperty("MaximalUsers")]
    public int MaximalUsers
    {
        get => _maximalUsers;
        set
        {
            if (_maximalUsers != value)
            {
                _maximalUsers = value;
                OnPropertyChanged();
            }
        }
    }

    [JsonProperty("PasswordSecured")] public bool PasswordSecured { get; set; }
    [JsonProperty("Password")] public string Password { get; set; }

    [JsonProperty("MiddleCard")] public CardDto MiddleCard { get; set; }
    [JsonProperty("MiddleCardPic")] public string MiddleCardPic { get; set; }
    [JsonProperty("SelectedCard")] public CardDto SelectedCard { get; set; }

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

    [JsonProperty("PlayerTurnId")] public int PlayerTurnId { get; set; }

    [JsonProperty("NextPlayer")] public int NextPlayer { get; set; }

    [JsonProperty("IsReverse")] public bool IsReverse { get; set; }

    [JsonProperty("IsSkip")] public bool IsSkip { get; set; }

    private ObservableCollection<CardViewModel> _cards = new();

    [JsonProperty("Cards")]
    public ObservableCollection<CardViewModel> Cards
    {
        get => _cards;
        set
        {
            if (Equals(value, _cards)) return;
            _cards = value;
            OnPropertyChanged();
        }
    }
}