using System.Collections.ObjectModel;
using UNO_Server.ViewModel;

namespace UNO_Server.Models;

public class PlayerItem
{
    public long Id { get; set; }
    public string? PlayerName { get; set; }
    public ObservableCollection<CardViewModel> Hand { get; set; } = new();
    public bool Uno { get; set; }
}