using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Window;
using UNO_Spielprojekt.Winner;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.MultiplayerRooms;

namespace UNO_Spielprojekt.MultiplayerGamePage;

public class MPGamePageViewModel : ViewModelBase
{
    private Brush _theBackground;

    public Brush TheBackground
    {
        get => _theBackground;
        set
        {
            _theBackground = value;
            OnPropertyChanged();
        }
    }

    public int SelectedCardIndex { get; set; }
    private int RoundCounter { get; set; }

    private string _roundCounterString;

    public string RoundCounterString
    {
        get => _roundCounterString;
        set
        {
            if (_roundCounterString != value)
            {
                _roundCounterString = value;
                OnPropertyChanged();
            }
        }
    }

    private ObservableCollection<CardViewModel> _currentHand = new ObservableCollection<CardViewModel>();

    public ObservableCollection<CardViewModel> CurrentHand
    {
        get => _currentHand;
        set
        {
            if (_currentHand != value)
            {
                _currentHand = value;
                OnPropertyChanged();
            }
        }
    }

    private CardViewModel SelectedCard { get; set; }

    private readonly MainViewModel _mainViewModel;
    private readonly ILogger _logger;
    private PlayViewModel PlayViewModel { get; }
    private GameLogic GameLogic { get; }
    private WinnerViewModel WinnerViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }

    public ScoreboardViewModel _scoreboardViewModel;

    public RelayCommand ZiehenCommand { get; }
    public RelayCommand LegenCommand { get; }
    public RelayCommand FertigCommand { get; }
    public RelayCommand UnoCommand { get; }
    public RelayCommand ExitConfirmCommand { get; }
    private HttpClient _httpClient;

    public MPGamePageViewModel(MainViewModel mainViewModel, ILogger logger, PlayViewModel playViewModel,
        GameLogic gameLogic, WinnerViewModel winnerViewModel, ScoreboardViewModel scoreboardViewModel,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        _httpClient = new HttpClient();
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        _scoreboardViewModel = scoreboardViewModel;
        GameLogic = gameLogic;
        WinnerViewModel = winnerViewModel;
        PlayViewModel = playViewModel;
        _mainViewModel = mainViewModel;
        _logger = logger;

        TheBackground = Brushes.Transparent;

        RoundCounter = 1;
        RoundCounterString = $"Runde: {RoundCounter}/\u221e";

        ZiehenCommand = new RelayCommand(ZiehenCommandMethod);
        // LegenCommand = new RelayCommand(LegenCommandMethod);
        // FertigCommand = new RelayCommand(FertigCommandMethod);
        // UnoCommand = new RelayCommand(UnoCommandMethod);
        // ExitConfirmCommand = new RelayCommand(ExitConfirmCommandMethod);
    }

    // public List<string> TestListe { get; set; }= new List<string>() {"sddsds", "dsdsdsas", "dsadsa" };

    private async Task DrawCard(Rooms roomToUpdate)
    {
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/drawCard/{MultiplayerRoomsViewModel.Player.Name}";

        var response = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();

        await MultiplayerRoomsViewModel.GetAllRooms();
    }
    private void ZiehenCommandMethod()
    {
        DrawCard(MultiplayerRoomsViewModel.SelectedRoom2);
        SetCurrentHand();
        // MultiplayerRoomsViewModel.Player.PlayerHand.Add(new CardViewModel(){ Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" });
        OnPropertyChanged(nameof(MultiplayerRoomsViewModel));
    }

    private bool _legen;
    private bool _ziehen;
    private ChooseColorViewModel _chooseColorViewModel;
    private bool _chooseColorVisible;

    public void LegenCommandMethod()
    {
        SelectedCard = MultiplayerRoomsViewModel.Player.PlayerHand[SelectedCardIndex];
    }

    public void SetCurrentHand()
    {
        if (MultiplayerRoomsViewModel.Player.PlayerHand != null)
        {
            CurrentHand.Clear();
            foreach (var card in MultiplayerRoomsViewModel.Player.PlayerHand)
            {
                CurrentHand.Add(card);
            }

            _logger.Info("CurrentHand wurde gesetzt.");
        }
    }
}