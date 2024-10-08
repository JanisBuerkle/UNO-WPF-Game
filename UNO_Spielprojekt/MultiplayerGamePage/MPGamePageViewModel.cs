using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.ChooseColor;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Window;
using UNO.Contract;

namespace UNO_Spielprojekt.MultiplayerGamePage;

public class MPGamePageViewModel : ViewModelBase
{
    private readonly ILogger logger;
    private Brush theBackground;

    private string roundCounterString;

    private ObservableCollection<CardDto> currentHand = new();

    private bool disableAllFunctions;
    private bool itsYourTurn;

    private bool chooseColorVisible;

    private ChooseColorViewModel chooseColorViewModel;

    private bool fertigButtonIsEnabled;

    private bool gelegt;
    private bool gezogen;

    public RelayCommand ZiehenCommand { get; }
    public RelayCommand FertigCommand { get; }

    public RelayCommand UnoCommand { get; }
    private MainViewModel MainViewModel { get; }

    private CardDto SelectedCard { get; set; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; set; }
    public int SelectedCardIndex { get; set; }
    public int MoveCounter { get; set; }

    // public RelayCommand ExitConfirmCommand { get; }
    public MPGamePageViewModel(MainViewModel mainViewModel, ILogger loggerr,
        MultiplayerRoomsViewModel multiplayerRoomsViewModel)
    {
        MultiplayerRoomsViewModel = multiplayerRoomsViewModel;
        MainViewModel = mainViewModel;
        logger = loggerr;

        RoundCounterString = $"Runde: {MoveCounter}/\u221e";
        logger.Info(
            "Der Hintergrund des Buttons TheBackground, MoveCounter und RoundCounterString wurden auf ihre Standartwerte gesetzt.");

        ZiehenCommand = new RelayCommand(ZiehenCommandMethod);
        FertigCommand = new RelayCommand(FertigCommandMethod);
        UnoCommand = new RelayCommand(UnoCommandMethod);
        // ExitConfirmCommand = new RelayCommand(ExitConfirmCommandMethod);
    }

    public async void LegenCommandMethod()
    {
        if (!gelegt)
        {
            logger.Info("Eine Karte wurde angeklickt, LegenCommandMethod wurde ausgeführt.");
            var saveMiddleCard = MultiplayerRoomsViewModel.SelectedRoom2.MiddleCard;
            SelectedCard = MultiplayerRoomsViewModel.Player.PlayerHand[SelectedCardIndex];

            MultiplayerRoomsViewModel.Player.PlayerHand.Remove(SelectedCard);
            var reinholen = SelectedCard.Color + "-" + SelectedCard.Value + "-" + SelectedCard.Id;

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
                logger.Info($"{SelectedCard.Color + SelectedCard.Value} wurde ausgespielt.");
            }

            await MultiplayerRoomsViewModel.GetRooms();
            SetCurrentHand();

            if (saveMiddleCard.Id != MultiplayerRoomsViewModel.SelectedRoom2.MiddleCard.Id)
            {
                gelegt = true;
                FertigButtonIsEnabled = true;
            }
        }
    }

    public void SetCurrentHand()
    {
        logger.Info("Die Hand des Spielers wurde geupdatet, SetCurrentHand wurde ausgeführt.");
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
        DisableAllFunctions = MainViewModel.MultiplayerRoomsViewModel.Player.Id ==
                              MultiplayerRoomsViewModel.SelectedRoom2.PlayerTurnId;
    }

    private async void UnoCommandMethod()
    {
        await MultiplayerRoomsViewModel.RoomClient.UnoClicked(MultiplayerRoomsViewModel.SelectedRoom2,
            (int)MultiplayerRoomsViewModel.Player.Id);
    }

    private async void ZiehenCommandMethod()
    {
        if (!gezogen && !gelegt)
        {
            logger.Info("Eine Karte wurde gezogen, ZiehenCommandMethod wurde ausgeführt.");
            var savePlayerCardCount = MultiplayerRoomsViewModel.Player.PlayerHand.Count;
            await MultiplayerRoomsViewModel.RoomClient.DrawCard(MultiplayerRoomsViewModel.Player.Name,
                MultiplayerRoomsViewModel.SelectedRoom2);
            await MultiplayerRoomsViewModel.GetRooms();

            SetCurrentHand();
            OnPropertyChanged(nameof(MultiplayerRoomsViewModel));

            if (savePlayerCardCount < MultiplayerRoomsViewModel.Player.PlayerHand.Count)
            {
                gezogen = true;
                FertigButtonIsEnabled = true;
            }
        }
    }

    private async void FertigCommandMethod()
    {
        if (gelegt || gezogen)
        {
            logger.Info("Fertig Button wurde geklickt, FertigCommandMethod wurde ausgeführt.");
            await MultiplayerRoomsViewModel.RoomClient.PlayerEndMove((int)MultiplayerRoomsViewModel.Player.Id,
                MultiplayerRoomsViewModel.SelectedRoom2);
            logger.Info($"{MultiplayerRoomsViewModel.PlayerName} hat seinen Zug beendet.");

            await MultiplayerRoomsViewModel.GetRooms();
            gelegt = false;
            gezogen = false;
            FertigButtonIsEnabled = false;
        }
    }

    private async void ColorChoosen(object? sender, PropertyChangedEventArgs e)
    {
        ChooseColorVisible = false;
        var reinholen = "";
        if (SelectedCard.Color == "Draw" || SelectedCard.Value == "Wild")
        {
            reinholen = SelectedCard.Color + "-" + SelectedCard.Value + "-" + SelectedCard.Id + "-" +
                        SelectedCard.Color + "-" + ChooseColorViewModel.ChoosenColor;
            gelegt = true;
            FertigButtonIsEnabled = true;
        }

        await MultiplayerRoomsViewModel.RoomClient.PlaceCard(reinholen, MultiplayerRoomsViewModel.SelectedRoom2);
        await MultiplayerRoomsViewModel.GetRooms();
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

    public ObservableCollection<CardDto> CurrentHand
    {
        get => currentHand;
        set
        {
            if (currentHand != value)
            {
                currentHand = value;
                OnPropertyChanged();
            }
        }
    }

    public bool DisableAllFunctions
    {
        get => disableAllFunctions;
        set
        {
            if (disableAllFunctions != value)
            {
                disableAllFunctions = value;
                ItsYourTurn = value;
                OnPropertyChanged();
            }
        }
    }

    public bool ItsYourTurn

    {
        get => itsYourTurn;
        set
        {
            if (itsYourTurn != value)
            {
                itsYourTurn = value;
                OnPropertyChanged();
            }
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

    public bool FertigButtonIsEnabled
    {
        get => fertigButtonIsEnabled;
        set
        {
            if (fertigButtonIsEnabled != value)
            {
                fertigButtonIsEnabled = value;
                TheBackground = !value ? Brushes.Transparent : Brushes.Green;
                OnPropertyChanged();
            }
        }
    }
}