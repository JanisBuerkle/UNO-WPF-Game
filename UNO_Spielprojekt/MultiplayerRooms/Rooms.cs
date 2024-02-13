using System.Collections.Generic;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.GamePage;

namespace UNO_Spielprojekt.MultiplayerRooms;

public class Rooms
{
    public string RoomName { get; set; }
    public int MaximalUsers { get; set;}
    public bool PasswordSecured { get; set;}
    public string Password { get; set;}
    public List<MultiplayerPlayer> PlayerNames { get; set; } = new List<MultiplayerPlayer>();
}