using System.Collections.ObjectModel;

namespace UNO_Server.ViewModel;

public class MultiplayerPlayer : ViewModelBase
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }

    private ObservableCollection<CardViewModel> _playerHand = new ObservableCollection<CardViewModel>();

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