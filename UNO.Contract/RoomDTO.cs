using System.Collections.ObjectModel;

namespace UNO.Contract;

public class RoomDto
{
    public long Id { get; set; }
    public string RoomName { get; set; }
    public bool PlayButtonEnabled { get; set; } = true;
    public string PlayButtonContent { get; set; } = "Play";
    public int OnlineUsers { get; set; }
    public ObservableCollection<CardDto> Center { get; set; } = new();
    public CardDto MiddleCard { get; set; } = new() { Color = String.Empty, Value = String.Empty, ImageUri = String.Empty };
    public string MiddleCardPic { get; set; } = String.Empty;
    public CardDto SelectedCard { get; set; } = new() { Color = String.Empty, Value = String.Empty, ImageUri = String.Empty };
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
    public List<PlayerDto> Players { get; set; } = new();
    public ObservableCollection<CardDto> Cards { get; set; } = new();
}