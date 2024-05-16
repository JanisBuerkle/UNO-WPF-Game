using System.Collections.ObjectModel;

namespace UNO.Contract;

public class RoomDTO
{
    public long Id { get; set; }
    public string RoomName { get; set; }
    public bool PlayButtonEnabled { get; set; } = true;
    public string PlayButtonContent { get; set; } = "Play";
    public int OnlineUsers { get; set; }
    public ObservableCollection<CardDTO> Center { get; set; } = new ObservableCollection<CardDTO>();
    public CardDTO MiddleCard { get; set; } = new CardDTO() {Color = "", Value = "", ImageUri = ""};
    public string MiddleCardPic { get; set; } = "";
    public CardDTO SelectedCard { get; set; } = new CardDTO() {Color = "", Value = "", ImageUri = ""};
    public int MaximalUsers { get; set; }
    public bool PasswordSecured { get; set; }
    public string Password { get; set; }
    public int PlayerTurnId { get; set; }
    public int NextPlayer { get; set; }
    public bool IsReverse { get; set; }
    public bool IsSkip { get; set; }
    public int MoveCounter { get; set; }
    public int StartingPlayer { get; set; }
    public List<PlayerDTO> Players { get; set; } = new List<PlayerDTO>();
    public ObservableCollection<CardDTO> Cards { get; set; } = new ObservableCollection<CardDTO>();
}