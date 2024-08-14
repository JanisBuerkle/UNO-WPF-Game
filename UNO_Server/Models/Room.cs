using System.Collections.ObjectModel;

namespace UNO_Server.Models;

public class Room
{
    public long Id { get; set; }
    public string RoomName { get; set; }
    public bool PlayButtonEnabled { get; set; } = true;
    public string PlayButtonContent { get; set; } = "Play";
    public int OnlineUsers { get; set; }
    public ObservableCollection<Card> Center { get; set; } = new();
    public Card MiddleCard { get; set; } = new() { Color = String.Empty, Value = String.Empty, ImageUri = String.Empty };
    public string MiddleCardPic { get; set; } = String.Empty;
    public Card SelectedCard { get; set; } = new() { Color = String.Empty, Value = String.Empty, ImageUri = String.Empty };
    public int MaximalUsers { get; set; }
    public bool PasswordSecured { get; set; }
    public string Password { get; set; }
    public int PlayerTurnId { get; set; }
    public int NextPlayer { get; set; }
    public bool IsReverse { get; set; }
    public bool IsSkip { get; set; }
    public int MoveCounter { get; set; }
    public bool StartButtonEnabled { get; set; }
    public int StartingPlayer { get; set; }
    public List<Player> Players { get; set; } = new();
    public ObservableCollection<Card> Cards { get; set; } = new();
}