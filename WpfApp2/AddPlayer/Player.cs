using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp2.GamePage;

namespace WpfApp2.AddPlayer;

// Player.cs
public class Player
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public List<CardViewModel> Hand { get; set; }
    public bool Uno { get; set; }
}