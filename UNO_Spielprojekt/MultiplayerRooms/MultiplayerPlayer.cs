using System.Collections.Generic;
using System.Collections.ObjectModel;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class MultiplayerPlayer : ViewModelBase
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }

    public bool IsLeader { get; set; }

    private List<CardViewModel> _playerHand = new();
    public List<CardViewModel> PlayerHand
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

    private bool Equals(MultiplayerPlayer other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((MultiplayerPlayer)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}