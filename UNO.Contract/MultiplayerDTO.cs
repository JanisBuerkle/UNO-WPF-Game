using System.Collections.ObjectModel;

namespace UNO.Contract;

public class MultiplayerDTO
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }
    public ObservableCollection<CardDTO> PlayerHand { get; set; }
}