using UNO_Spielprojekt.MultiplayerRooms;
using System.Collections.ObjectModel;
using UNO_Spielprojekt.Scoreboard;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;
using UNO_Spielprojekt.Winner;
using System.Threading.Tasks;
using System.Windows.Media;
using tt.Tools.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using UNO.Contract;
using System.Text;

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

    private ObservableCollection<CardDTO> _currentHand = new ObservableCollection<CardDTO>();

    public ObservableCollection<CardDTO> CurrentHand
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

    private CardDTO SelectedCard { get; set; }

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
    private readonly HttpClient _httpClient;

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

    private async Task DrawCard(RoomDTO roomToUpdate)
    {
        var jsonContent = JsonConvert.SerializeObject(roomToUpdate);
        var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

        var addPlayerUrl = $"http://localhost:5000/api/Rooms/drawCard/{MultiplayerRoomsViewModel.Player.Name}";

        var response = await _httpClient.PutAsync(addPlayerUrl, httpContent);
        response.EnsureSuccessStatusCode();

        await MultiplayerRoomsViewModel.GetRooms();
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