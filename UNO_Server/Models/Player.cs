using System.Collections.ObjectModel;

namespace UNO_Server.Models;

public class Player
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }
    public ObservableCollection<Card> PlayerHand { get; set; } = new ObservableCollection<Card>();
}