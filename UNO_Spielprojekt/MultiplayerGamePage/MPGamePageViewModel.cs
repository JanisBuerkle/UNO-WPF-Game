using UNO_Spielprojekt.MultiplayerRooms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.Window;
using System.Windows.Media;
using tt.Tools.Logging;
using UNO_Spielprojekt.GamePage;
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

    private bool _disableAllFunctions;

    public bool DisableAllFunctions
    {
        get => _disableAllFunctions;
        set
        {
            if (_disableAllFunctions != value)
            {
                _disableAllFunctions = value;
                ItsYourTurn = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _itsYourTurn;

    public bool ItsYourTurn
    {
        get => _itsYourTurn;
        set
        {
            if (_itsYourTurn != value)
            {
                _itsYourTurn = value;
                OnPropertyChanged();
            }
        }
    }

    private bool _chooseColorVisible;

    public bool ChooseColorVisible
    {
        get => _chooseColorVisible;
        set
        {
            if (value == _chooseColorVisible) return;
            _chooseColorVisible = value;
            OnPropertyChanged();
        }
    }

    private ChooseColorViewModel _chooseColorViewModel;

    public ChooseColorViewModel ChooseColorViewModel
    {
        get => _chooseColorViewModel;
        set
        {
            if (Equals(value, _chooseColorViewModel)) return;
            _chooseColorViewModel = value;
            OnPropertyChanged();
        }
    }

    private bool _fertigButtonIsEnabled;

    public bool FertigButtonIsEnabled
    {
        get => _fertigButtonIsEnabled;
        set
        {
            if (_fertigButtonIsEnabled != value)
            {
                _fertigButtonIsEnabled = value;
                TheBackground = !value ? Brushes.Transparent : Brushes.Green;
                OnPropertyChanged();
            }
        }
    }

    private CardDTO SelectedCard { get; set; }
    private MainViewModel MainViewModel { get; set; }
    private readonly ILogger _logger;
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public int SelectedCardIndex { get; set; }
    public int MoveCounter { get; set; }

    public RelayCommand ZiehenCommand { get; }
    public RelayCommand FertigCommand { get; }
    public RelayCommand UnoCommand { get; }
    // public RelayCommand ExitConfirmCommand { get; }

    public MPGamePageViewModel(MainViewModel mainViewModel, ILogger logger,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        MainViewModel = mainViewModel;
        _logger = logger;
        
        RoundCounterString = $"Runde: {MoveCounter}/\u221e";
        _logger.Info(
            "Der Hintergrund des Buttons TheBackground, MoveCounter und RoundCounterString wurden auf ihre Standartwerte gesetzt.");

        ZiehenCommand = new RelayCommand(ZiehenCommandMethod);
        FertigCommand = new RelayCommand(FertigCommandMethod);
        UnoCommand = new RelayCommand(UnoCommandMethod);
        // ExitConfirmCommand = new RelayCommand(ExitConfirmCommandMethod);
    }

    private bool _gelegt;
    private bool _gezogen;

    private async void UnoCommandMethod()
    {
        await MultiplayerRoomsViewModel.RoomClient.UnoClicked(MultiplayerRoomsViewModel.SelectedRoom2,
            (int)MultiplayerRoomsViewModel.Player.Id);
    }

    private async void ZiehenCommandMethod()
    {
        if (!_gezogen && !_gelegt)
        {
            _logger.Info("Eine Karte wurde gezogen, ZiehenCommandMethod wurde ausgeführt.");
            var savePlayerCardCount = MultiplayerRoomsViewModel.Player.PlayerHand.Count;
            await MultiplayerRoomsViewModel.RoomClient.DrawCard(MultiplayerRoomsViewModel.Player.Name,
                MultiplayerRoomsViewModel.SelectedRoom2);
            await MultiplayerRoomsViewModel.GetRooms();

            SetCurrentHand();
            OnPropertyChanged(nameof(MultiplayerRoomsViewModel));

            if (savePlayerCardCount < MultiplayerRoomsViewModel.Player.PlayerHand.Count)
            {
                _gezogen = true;
                FertigButtonIsEnabled = true;
            }
        }
    }

    public async void LegenCommandMethod()
    {
        if (!_gelegt)
        {
            _logger.Info("Eine Karte wurde angeklickt, LegenCommandMethod wurde ausgeführt.");
            var saveMiddleCard = MultiplayerRoomsViewModel.SelectedRoom2.MiddleCard;
            SelectedCard = MultiplayerRoomsViewModel.Player.PlayerHand[SelectedCardIndex];

            MultiplayerRoomsViewModel.Player.PlayerHand.Remove(SelectedCard);
            string reinholen = SelectedCard.Color + "-" + SelectedCard.Value + "-" + SelectedCard.Id;

            if (SelectedCard.Color == "Draw" || SelectedCard.Value == "Wild")
            {
                ChooseColorViewModel = new ChooseColorViewModel();
                ChooseColorViewModel.PropertyChanged += ColorChoosen;
                ChooseColorVisible = true;
            }
            else
            {
                await MultiplayerRoomsViewModel.RoomClient.PlaceCard(reinholen,
                    MultiplayerRoomsViewModel.SelectedRoom2);
                _logger.Info($"{SelectedCard.Color + SelectedCard.Value} wurde ausgespielt.");
            }

            await MultiplayerRoomsViewModel.GetRooms();
            SetCurrentHand();

            if (saveMiddleCard.Id != MultiplayerRoomsViewModel.SelectedRoom2.MiddleCard.Id)
            {
                _gelegt = true;
                FertigButtonIsEnabled = true;
            }
        }
    }

    private async void FertigCommandMethod()
    {
        if (_gelegt || _gezogen)
        {
            _logger.Info("Fertig Button wurde geklickt, FertigCommandMethod wurde ausgeführt.");
            await MultiplayerRoomsViewModel.RoomClient.PlayerEndMove((int)MultiplayerRoomsViewModel.Player.Id,
                MultiplayerRoomsViewModel.SelectedRoom2);
            _logger.Info($"{MultiplayerRoomsViewModel.PlayerName} hat seinen Zug beendet.");

            await MultiplayerRoomsViewModel.GetRooms();
            _gelegt = false;
            _gezogen = false;
            FertigButtonIsEnabled = false;
        }
    }

    public void SetCurrentHand()
    {
        _logger.Info("Die Hand des Spielers wurde geupdatet, SetCurrentHand wurde ausgeführt.");
        if (MultiplayerRoomsViewModel.Player.PlayerHand != null)
        {
            CurrentHand.Clear();
            foreach (var card in MultiplayerRoomsViewModel.Player.PlayerHand)
            {
                CurrentHand.Add(card);
            }
        }
    }

    public void DiableAllFunctions()
    {
        if (MainViewModel.MultiplayerRoomsViewModel.Player.Id != MultiplayerRoomsViewModel.SelectedRoom2.PlayerTurnId)
        {
            DisableAllFunctions = false;
        }
        else
        {
            DisableAllFunctions = true;
        }
    }

    private async void ColorChoosen(object? sender, PropertyChangedEventArgs e)
    {
        ChooseColorVisible = false;
        string reinholen = "";
        if (SelectedCard.Color == "Draw")
        {
            reinholen = SelectedCard.Color + "-" + SelectedCard.Value + "-" + SelectedCard.Id + "-" + "Draw" + "-" +
                        ChooseColorViewModel.ChoosenColor;
            _gelegt = true;
            FertigButtonIsEnabled = true;
        }
        else if (SelectedCard.Value == "Wild")
        {
            reinholen = SelectedCard.Color + "-" + SelectedCard.Value + "-" + SelectedCard.Id + "-" + "Wild" + "-" +
                        ChooseColorViewModel.ChoosenColor;
            _gelegt = true;
            FertigButtonIsEnabled = true;
        }

        await MultiplayerRoomsViewModel.RoomClient.PlaceCard(reinholen, MultiplayerRoomsViewModel.SelectedRoom2);
        await MultiplayerRoomsViewModel.GetRooms();
    }
}