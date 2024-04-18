using UNO_Spielprojekt.MultiplayerRooms;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.Window;
using System.Windows.Media;
using tt.Tools.Logging;
using UNO.Contract;

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
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }

    public RelayCommand ZiehenCommand { get; }
    // public RelayCommand LegenCommand { get; }
    // public RelayCommand FertigCommand { get; }
    // public RelayCommand UnoCommand { get; }
    // public RelayCommand ExitConfirmCommand { get; }

    public MPGamePageViewModel(MainViewModel mainViewModel, ILogger logger,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        _mainViewModel = mainViewModel;
        _logger = logger;

        TheBackground = Brushes.Transparent;
        RoundCounter = 1;
        RoundCounterString = $"Runde: {RoundCounter}/\u221e";
        _logger.Info(
            "Der Hintergrund des Buttons TheBackground, RoundCounter und RoundCounterString wurden auf ihre Standartwerte gesetzt.");

        ZiehenCommand = new RelayCommand(ZiehenCommandMethod);
        // LegenCommand = new RelayCommand(LegenCommandMethod);
        // FertigCommand = new RelayCommand(FertigCommandMethod);
        // UnoCommand = new RelayCommand(UnoCommandMethod);
        // ExitConfirmCommand = new RelayCommand(ExitConfirmCommandMethod);
    }

    // private bool _legen;
    // private bool _ziehen;
    // private ChooseColorViewModel _chooseColorViewModel;
    // private bool _chooseColorVisible;

    private async void ZiehenCommandMethod()
    {
        _logger.Info("Eine Karte wurde gezogen, ZiehenCommandMethod wurde ausgeführt.");
        await MultiplayerRoomsViewModel.RoomClient.DrawCard(MultiplayerRoomsViewModel.Player.Name,
            MultiplayerRoomsViewModel.SelectedRoom2);
        await MultiplayerRoomsViewModel.GetRooms();

        SetCurrentHand();
        OnPropertyChanged(nameof(MultiplayerRoomsViewModel));
    }

    public async void LegenCommandMethod()
    {
        _logger.Info("Eine Karte wurde angeklickt, LegenCommandMethod wurde ausgeführt.");
        SelectedCard = MultiplayerRoomsViewModel.Player.PlayerHand[SelectedCardIndex];
        foreach (var card in MultiplayerRoomsViewModel.Player.PlayerHand)
        {
            if (card == SelectedCard)
            {
                MultiplayerRoomsViewModel.Player.PlayerHand.Remove(card);
                await MultiplayerRoomsViewModel.RoomClient.PlaceCard(card.Color + card.Value,
                    MultiplayerRoomsViewModel.SelectedRoom2);
                _logger.Info($"{card.Color + card.Value} wurde ausgespielt.");
                return;
            }
        }

        await MultiplayerRoomsViewModel.GetRooms();
        SetCurrentHand();
    }

    public void SetCurrentHand()
    {
        _logger.Info("Die Hand des Spielers wurde geupdatet, SetCurrentHand wurde ausgeführt.");
        if (MultiplayerRoomsViewModel.Player.PlayerHand != null)
        {
            var unused = MultiplayerRoomsViewModel.SelectedRoom2.MiddleCard; //test
            CurrentHand.Clear();
            foreach (var card in MultiplayerRoomsViewModel.Player.PlayerHand)
            {
                CurrentHand.Add(card);
            }
        }
    }
}