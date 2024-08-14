using System.Collections.ObjectModel;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerPlayer : ViewModelBase
{
    private ObservableCollection<CardViewModel> _playerHand = new();
    public long Id { get; set; }
    public string ConnectionId { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }

    public ObservableCollection<CardViewModel> PlayerHand
    {
        get => _playerHand;
        set
        {
            if (_playerHand != value)
            {
                _playerHand = value;
                OnPropertyChanged();
            }
        }
    }
}