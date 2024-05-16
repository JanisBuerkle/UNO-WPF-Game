using System.Collections.ObjectModel;

namespace UNO.Contract;

public class PlayerDTO
{
    public long Id { get; set; }
    public long RoomId { get; set; }
    public string Name { get; set; }
    public bool IsLeader { get; set; }
    public bool Uno { get; set; }
    public ObservableCollection<CardDTO> PlayerHand { get; set; }
}