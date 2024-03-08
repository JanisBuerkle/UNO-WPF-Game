using System.Collections.ObjectModel;
using UNO_Server.ViewModel;

namespace UNO_Server.Models;

public class RoomItem : ViewModelBase
{
    public long Id { get; set; }
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
    
    private int _maximalUsers;
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
    public bool PasswordSecured { get; set;}
    public string Password { get; set;}
    public List<MultiplayerPlayer> Players { get; set; } = new List<MultiplayerPlayer>();
    
    private ObservableCollection<CardViewModel> _cards = new();
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