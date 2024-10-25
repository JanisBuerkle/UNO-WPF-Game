using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.ChooseColor;
using UNO_Spielprojekt.ExitConfirm;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Window;
using UNO_Spielprojekt.Winner;

namespace UNO_Spielprojekt.GamePage;

public class GameViewModel : ViewModelBase
{
    private readonly Random random = new();

    private readonly MainViewModel mainViewModel;
    private readonly ILogger logger;

    private Brush theBackground;

    private CardViewModel middleCard;

    private string middleCardPic;

    public ScoreboardViewModel ScoreboardViewModel;

    private string roundCounterString;

    private string currentPlayerName;

    private bool legen;
    private bool ziehen;
    private ChooseColorViewModel chooseColorViewModel;
    private bool chooseColorVisible;
    private PlayViewModel PlayViewModel { get; }
    private GameLogic GameLogic { get; }
    private WinnerViewModel WinnerViewModel { get; }

    public RelayCommand ZiehenCommand { get; }
    public RelayCommand LegenCommand { get; }
    public RelayCommand FertigCommand { get; }
    public RelayCommand UnoCommand { get; }
    public RelayCommand ExitConfirmCommand { get; }
    private int StartingPlayer { get; set; }
    private int CurrentPlayer { get; set; }
    private int NextPlayer { get; set; }
    public ObservableCollection<CardViewModel> CurrentHand { get; set; } = new();
    public int SelectedCardIndex { get; set; }
    private int MoveCounter { get; set; }

    private bool IsSkip { get; set; }
    private bool IsReverse { get; set; }
    private CardViewModel SelectedCard { get; set; }

    private bool IsEnd { get; set; }


    public GameViewModel(MainViewModel mainViewModel, ILogger loggerr, PlayViewModel playViewModel, GameLogic gameLogic,
        WinnerViewModel winnerViewModel, ScoreboardViewModel scoreboardViewModel)
    {
        ScoreboardViewModel = scoreboardViewModel;
        TheBackground = Brushes.Transparent;
        GameLogic = gameLogic;
        WinnerViewModel = winnerViewModel;
        PlayViewModel = playViewModel;
        this.mainViewModel = mainViewModel;
        logger = loggerr;

        ZiehenCommand = new RelayCommand(ZiehenCommandMethod);
        LegenCommand = new RelayCommand(LegenCommandMethod);
        FertigCommand = new RelayCommand(FertigCommandMethod);
        UnoCommand = new RelayCommand(UnoCommandMethod);

        ExitConfirmCommand = new RelayCommand(ExitConfirmCommandMethod);
        MoveCounter = 1;
        IsEnd = false;
        RoundCounterString = $"Runde: {MoveCounter}/\u221e";
    }

    public void LegenCommandMethod()
    {
        SelectedCard = GameLogic.Players[CurrentPlayer].Hand[SelectedCardIndex];
        if (!legen)
        {
            logger.Info($"{CurrentPlayerName} hat {SelectedCard.Color} {SelectedCard.Value} angeklickt.");
            if (SelectedCard.Value == "Wild")
            {
                TheBackground = Brushes.DarkSeaGreen;
                legen = true;

                ChooseColorViewModel = new ChooseColorViewModel();
                ChooseColorViewModel.PropertyChanged += ColorChoosen;
                ChooseColorVisible = true;
                SetCurrentHand();
                logger.Info($"{CurrentPlayerName} hat Wild gespielt.");
            }
            else if (SelectedCard.Value == "+4")
            {
                TheBackground = Brushes.DarkSeaGreen;
                legen = true;

                ChooseColorViewModel = new ChooseColorViewModel();
                ChooseColorViewModel.PropertyChanged += ColorChoosen;
                ChooseColorVisible = true;
                
                for (var i = 0; i < 4; i++)
                {
                    GameLogic.Players[NextPlayer].Hand.Add(PlayViewModel.Cards[0]);
                    PlayViewModel.Cards.RemoveAt(0);
                }

                SetCurrentHand();
                logger.Info($"{CurrentPlayerName} hat {SelectedCard.Color} +4 gespielt.");
            }
            else if (SelectedCard.Color == GameLogic.Center[^1].Color || SelectedCard.Value == GameLogic.Center[^1].Value)
            {
                TheBackground = Brushes.DarkSeaGreen;
                legen = true;
                GameLogic.Players[CurrentPlayer].Hand.RemoveAt(SelectedCardIndex);
                GameLogic.Center.Add(SelectedCard);
                MiddleCardPic = SelectedCard.ImageUri;
                SetCurrentHand();
                logger.Info($"{CurrentPlayerName} hat {SelectedCard.Color} {SelectedCard.Value} gespielt.");
            }

            if (SelectedCard.Value == "Skip" && SelectedCard.Value == GameLogic.Center[^1].Value)
            {
                TheBackground = Brushes.DarkSeaGreen;
                IsSkip = true;
                SetCurrentHand();
            }

            else if (SelectedCard.Value == "+2" && SelectedCard.Value == GameLogic.Center[^1].Value)
            {
                TheBackground = Brushes.DarkSeaGreen;
                for (var i = 0; i < 2; i++)
                {
                    GameLogic.Players[NextPlayer].Hand.Add(PlayViewModel.Cards[0]);
                    PlayViewModel.Cards.RemoveAt(0);
                }

                SetCurrentHand();
            }
            else if (SelectedCard.Value == "Reverse" && SelectedCard.Value == GameLogic.Center[^1].Value)
            {
                TheBackground = Brushes.DarkSeaGreen;
                IsReverse = !IsReverse;
                if (GameLogic.Players.Count == 2)
                {
                    if (CurrentPlayer == 0)
                    {
                        CurrentPlayer = GameLogic.Players.Count - 1;
                    }
                    else
                    {
                        CurrentPlayer--;
                    }
                }
            }
        }
    }

    public void Game()
    {
        mainViewModel.GoToGame();
    }

    public void SetCurrentHand()
    {
        logger.Info("CurrentHand wurde gesetzt.");
        if (GameLogic.Players.Count != 0)
        {
            CurrentHand.Clear();
            foreach (var card in GameLogic.Players[CurrentPlayer].Hand)
            {
                CurrentHand.Add(card);
            }
        }

        CurrentPlayerName = GameLogic.Players[CurrentPlayer].PlayerName;
    }

    public List<ScoreboardPlayer> LoadPlayersFromXml(string path)
    {
        var serializer = new XmlSerializer(typeof(List<ScoreboardPlayer>));
        var fileStream = new FileStream(Path.Combine(path), FileMode.Open);
        var x = (List<ScoreboardPlayer>)serializer.Deserialize(fileStream)!;
        fileStream.Close();
        return x;
    }

    public void InitializeGame()
    {
        InitializePlayersHands();
        InitializeGameProperties();
        GameLogic.PlaceFirstCardInCenter();

        MiddleCard = GameLogic.Center.First();
        SelectedCard = MiddleCard;
        if (MiddleCard.Color == "Wild" || MiddleCard.Color == "Draw")
        {
            ChooseColorViewModel = new ChooseColorViewModel();
            ChooseColorViewModel.PropertyChanged += ColorChoosen;
            ChooseColorVisible = true;
            GameLogic.Center.RemoveAt(0);
        }

        var middleCardPath = MiddleCard.ImageUri;
        MiddleCardPic = middleCardPath;
    }


    private void ZiehenCommandMethod()
    {
        if (!ziehen && !legen)
        {
            TheBackground = Brushes.DarkSeaGreen;
            var card = PlayViewModel.Cards.First();
            PlayViewModel.Cards.RemoveAt(0);
            GameLogic.Players[CurrentPlayer].Hand.Add(card);
            SetCurrentHand();
            ziehen = true;
            logger.Info($"{CurrentPlayerName} hat eine Karte gezogen {card.Color} {card.Value}.");
        }
    }

    private void ColorChoosen(object? sender, PropertyChangedEventArgs e)
    {
        ChooseColorVisible = false;
        GameLogic.Players[CurrentPlayer].Hand.RemoveAt(SelectedCardIndex);

        if (SelectedCard.Value == "Wild")
        {
            GameLogic.Center.Add(GameLogic.WildCards[ChooseColorViewModel.ChoosenColor]);
            MiddleCardPic = GameLogic.WildCards[ChooseColorViewModel.ChoosenColor].ImageUri;
        }
        else
        {
            GameLogic.Center.Add(GameLogic.Draw4Cards[ChooseColorViewModel.ChoosenColor]);
            MiddleCardPic = GameLogic.Draw4Cards[ChooseColorViewModel.ChoosenColor].ImageUri;
        }

        SetCurrentHand();
        logger.Info($"{CurrentPlayerName} hat eine Farbe ausgewählt.");
    }

    private void FertigCommandMethod()
    {
        CheckForWinner();
        if (!IsEnd)
        {
            if (legen || ziehen)
            {
                CheckForDrawStack();
                legen = false;
                ziehen = false;
                TheBackground = Brushes.Transparent;
                logger.Info($"{CurrentPlayerName} hat seinen Zug beendet.");
                if (GameLogic.Players[CurrentPlayer].Hand.Count <= 1)
                {
                    UnoCommandMethod();
                }

                if (IsReverse)
                {
                    IsReverseEnd();
                }
                else if (!IsReverse)
                {
                    IsNotReverseEnd();
                }

                SetCurrentHand();
                CurrentPlayerName = GameLogic.Players[CurrentPlayer].PlayerName;
            }
        }
    }

    private void IsReverseEnd()
    {
        if (CurrentPlayer == 0)
        {
            CurrentPlayer = GameLogic.Players.Count - 1;
            if (IsSkip)
            {
                CurrentPlayer = GameLogic.Players.Count - 2;
                IsSkip = false;
            }

            if (CurrentPlayer == 0)
            {
                NextPlayer = GameLogic.Players.Count - 1;
            }
            else
            {
                NextPlayer = CurrentPlayer - 1;
            }

            logger.Info("Eine neue Runde hat begonnen.");
            MoveCounter++;
            RoundCounterString = $"Runde: {MoveCounter}/\u221e";
        }
        else
        {
            CurrentPlayer--;
            if (IsSkip)
            {
                if (CurrentPlayer == 0)
                {
                    CurrentPlayer = GameLogic.Players.Count - 1;
                }

                IsSkip = false;
            }

            if (CurrentPlayer == 0)
            {
                NextPlayer = GameLogic.Players.Count - 1;
            }
        }
    }
    private void IsNotReverseEnd()
    {
        if (CurrentPlayer == GameLogic.Players.Count - 1)
        {
            CurrentPlayer = 0;
            if (IsSkip)
            {
                CurrentPlayer += 1;
                IsSkip = false;
            }

            if (CurrentPlayer == GameLogic.Players.Count - 1)
            {
                NextPlayer = 0;
            }
            else
            {
                NextPlayer = CurrentPlayer + 1;
            }

            logger.Info("Neue Runde hat begonnen.");
            MoveCounter++;
            RoundCounterString = $"Runde: {MoveCounter}/\u221e";
        }
        else
        {
            CurrentPlayer++;
            if (IsSkip)
            {
                if (CurrentPlayer == GameLogic.Players.Count - 1)
                {
                    CurrentPlayer = 0;
                }

                IsSkip = false;
            }

            if (CurrentPlayer == GameLogic.Players.Count - 1)
            {
                NextPlayer = 0;
            }
            else
            {
                NextPlayer = CurrentPlayer + 1;
            }
        }
    }

    private void UnoCommandMethod()
    {
        if (GameLogic.Players[CurrentPlayer].Uno == false)
        {
            if (GameLogic.Players[CurrentPlayer].Hand.Count <= 1)
            {
                logger.Info("UNO wurde gerufen!");
                GameLogic.Players[CurrentPlayer].Uno = true;
            }
            else
            {
                logger.Info("Uno wurde gedrückt, ohne das diese Person 1 Karte hatte.");
            }
        }
        logger.Info("UNO wurde vergessen zu drücken.");
    }

    private void ExitConfirmCommandMethod()
    {
        // Wrong layer. Needs to be in View Layer
        var exitConfirmWindow = new ExitConfirmWindow(mainViewModel)
        {
            Owner = MainWindowView.Instance
        };
        exitConfirmWindow.ShowDialog();
    }

    private void CheckForDrawStack()
    {
        if (PlayViewModel.Cards.Count <= 5)
        {
            var card1 = GameLogic.Center.Last();
            GameLogic.Center.RemoveAt(GameLogic.Center.Count - 1);
            foreach (var card in GameLogic.Center)
            {
                if (card.Value == "+4")
                {
                    card.Color = "Draw";
                    card.ImageUri = "pack://application:,,,/Assets/cards/+4/Draw.png";
                }
                else if (card.Value == "Wild")
                {
                    card.Color = "Wild";
                    card.ImageUri = "pack://application:,,,/Assets/cards/Wild/Wild.png";
                }

                PlayViewModel.Cards.Add(card);
            }

            var number = PlayViewModel.Cards.Count;
            while (number > 1)
            {
                number--;
                var card = random.Next(number + 1);
                (PlayViewModel.Cards[card], PlayViewModel.Cards[number]) =
                    (PlayViewModel.Cards[number], PlayViewModel.Cards[card]);
            }

            GameLogic.Center.Clear();
            GameLogic.Center.Add(card1);
            logger.Info("Center wird zu Deck hinzugefügt. (Mischen)");
        }
    }

    private void ResetAllPropertys()
    {
        GameLogic.Center.Clear();
        GameLogic.Players.Clear();
        PlayViewModel.Cards.Clear();
        CurrentHand.Clear();
        IsReverse = false;
        IsSkip = false;
        ziehen = false;
        legen = false;
        MoveCounter = 0;
    }

    private void CheckForWinner()
    {
        if (GameLogic.Players[CurrentPlayer].Uno == false)
        {
            if (GameLogic.Players[CurrentPlayer].Hand.Count <= 1)
            {
                var card = PlayViewModel.Cards.First();
                PlayViewModel.Cards.RemoveAt(0);
                GameLogic.Players[CurrentPlayer].Hand.Add(card);
                SetCurrentHand();
                GameLogic.Players[CurrentPlayer].Uno = true;
                ziehen = true;
                logger.Info($"{CurrentPlayerName} hat vergessen Uno zu drücken.");
            }
        }

        if (GameLogic.Players[CurrentPlayer].Hand.Count == 0)
        {
            logger.Info($"{CurrentPlayerName} hat Gewonnen!");
            WinnerViewModel.WinnerName = CurrentPlayerName;
            WinnerViewModel.MoveCounter = MoveCounter.ToString();
            mainViewModel.GoToWinner();
            IsEnd = true;

            if (File.Exists("GameData.xml"))
            {
                var players = LoadPlayersFromXml("GameData.xml");
                var existingPlayers =
                    players.Find(player => player.PlayerScoreboardName == CurrentPlayerName)!;

                if (existingPlayers != null)
                {
                    existingPlayers.PlayerScoreboardScore++;
                }
                else
                {
                    players.Add(new ScoreboardPlayer { PlayerScoreboardName = CurrentPlayerName, PlayerScoreboardScore = 1 });
                }

                SavePlayerToXml(players);
            }
            else
            {
                var players = new List<ScoreboardPlayer>();
                players.Add(new ScoreboardPlayer { PlayerScoreboardName = CurrentPlayerName, PlayerScoreboardScore = 1 });
                SavePlayerToXml(players);
            }

            ResetAllPropertys();
            mainViewModel.GameData.Load();
        }
    }

    private void SavePlayerToXml(List<ScoreboardPlayer> players)
    {
        var serializer = new XmlSerializer(typeof(List<ScoreboardPlayer>));
        using TextWriter writer = new StreamWriter("GameData.xml");
        serializer.Serialize(writer, players);
        writer.Close();
    }

    private void InitializeGameProperties()
    {
        StartingPlayer = GameLogic.ChooseStartingPlayer();
        CurrentPlayer = StartingPlayer;
        GameLogic.ShuffleDeck();
        GameLogic.DealCards(7);
        IsEnd = false;
    }

    public void InitializePlayersHands()
    {
        foreach (var cards in GameLogic.Cards)
        {
            PlayViewModel.Cards.Add(cards);
        }
    }

    public Brush TheBackground
    {
        get => theBackground;
        set
        {
            theBackground = value;
            OnPropertyChanged();
        }
    }

    private CardViewModel MiddleCard
    {
        get => middleCard;
        set
        {
            if (middleCard != value)
            {
                middleCard = value;
                OnPropertyChanged();
            }
        }
    }

    public string MiddleCardPic
    {
        get => middleCardPic;
        set
        {
            if (middleCardPic != value)
            {
                middleCardPic = value;
                OnPropertyChanged();
            }
        }
    }

    public string RoundCounterString
    {
        get => roundCounterString;
        set
        {
            if (roundCounterString != value)
            {
                roundCounterString = value;
                OnPropertyChanged();
            }
        }
    }

    public string CurrentPlayerName
    {
        get => currentPlayerName;
        set
        {
            if (currentPlayerName != value)
            {
                currentPlayerName = value;
                OnPropertyChanged();
            }
        }
    }

    public ChooseColorViewModel ChooseColorViewModel
    {
        get => chooseColorViewModel;
        set
        {
            if (Equals(value, chooseColorViewModel))
            {
                return;
            }

            chooseColorViewModel = value;
            OnPropertyChanged();
        }
    }

    public bool ChooseColorVisible
    {
        get => chooseColorVisible;
        set
        {
            if (value == chooseColorVisible)
            {
                return;
            }

            chooseColorVisible = value;
            OnPropertyChanged();
        }
    }
}