using System.Collections.Generic;

namespace UNO_Spielprojekt.GamePage;

public class Players
{
    public List<CardViewModel> Hand = new();
    public string PlayerName { get; set; }
    public bool Uno { get; set; }
}