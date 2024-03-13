using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;
using UNO_Spielprojekt.Winner;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
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
            OnPropertyChanged(nameof(TheBackground));
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

    public ObservableCollection<CardViewModel> CurrentHand { get; set; } = new();
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


    public MPGamePageViewModel(MainViewModel mainViewModel, ILogger logger, PlayViewModel playViewModel,
        GameLogic gameLogic, WinnerViewModel winnerViewModel, ScoreboardViewModel scoreboardViewModel,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
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

    private void ZiehenCommandMethod()
    {
        MultiplayerRoomsViewModel.Player.PlayerHand.Add(new CardViewModel(){ Color = "Blue", Value = "2", ImageUri = "pack://application:,,,/Assets/cards/2/Blue.png" });
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
}