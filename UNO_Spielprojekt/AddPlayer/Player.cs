using System.Collections.Generic;
using System.Collections.ObjectModel;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.AddPlayer;

// Player.cs
public class Player
{
    public int Id { get; set; }
    public string PlayerName { get; set; }
    public List<CardViewModel> Hand { get; set; }
    public bool Uno { get; set; }
}