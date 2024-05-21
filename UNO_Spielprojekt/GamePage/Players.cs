using System.Collections.Generic;

namespace UNO_Spielprojekt.GamePage;

public class Players
{
    public string PlayerName { get; set; }
    public List<CardViewModel> Hand = new();
    public bool Uno { get; set; }
}