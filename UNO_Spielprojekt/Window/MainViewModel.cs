using tt.Tools.Logging;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.MainMenu;
using UNO_Spielprojekt.MultiplayerCreateRoom;
using UNO_Spielprojekt.MultiplayerGamePage;
using UNO_Spielprojekt.MultiplayerGiveName;
using UNO_Spielprojekt.MultiplayerLobby;
using UNO_Spielprojekt.MultiplayerPassword;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Rules;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Service;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Winner;
using UNO.Contract;

namespace UNO_Spielprojekt.Window;

public class MainViewModel : ViewModelBase
{
    private readonly PlayViewModel playViewModel;

    private bool gameVisible;
    private bool rulesVisible;
    private bool winnerVisible;
    private bool settingsVisible;
    private bool mainMenuVisible;
    private bool addPlayerVisible;
    private bool scoreboardVisible;
    private bool multiplayerRoomsVisible;
    private bool multiplayerGamePageVisible;
    private bool createRoomVisible;
    private bool passwordVisible;
    private bool giveNameVisible;
    private bool lobbyVisible;

    public GameLogic GameLogic { get; set; }
    public WinnerViewModel WinnerViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; }
    public MPGamePageViewModel MultiplayerGamePageViewModel { get; }
    public LobbyViewModel LobbyViewModel { get; }
    public SettingsViewModel SettingsViewModel { get; }
    public ScoreboardViewModel ScoreboardViewModel { get; }
    public CreateRoomViewModel CreateRoomViewModel { get; set; }
    public PasswordViewModel PasswordViewModel { get; set; }
    public GiveNameViewModel GiveNameViewModel { get; set; }
    public GameData GameData { get; set; }
    public GameViewModel GameViewModel { get; set; }
    public RulesViewModel RulesViewModel { get; set; }
    public MainMenuViewModel MainMenuViewModel { get; set; }
    public AddPlayerViewModel AddPlayerViewModel { get; set; }

    public MainViewModel(IRoomClient roomClient, ILoggerFactory loggerFactory)
    {
        var logger = loggerFactory.CreateLogger("Uno-Spielprojekt");
        
        SettingsViewModel = new SettingsViewModel(this, logger);
        playViewModel = new PlayViewModel();
        GameLogic = new GameLogic(playViewModel, logger);
        ScoreboardViewModel = new ScoreboardViewModel(this);
        MultiplayerRoomsViewModel = new MultiplayerRoomsViewModel(this, logger, roomClient);
        WinnerViewModel = new WinnerViewModel(this, MultiplayerRoomsViewModel);
        GameViewModel = new GameViewModel(this, logger, playViewModel, GameLogic, WinnerViewModel, ScoreboardViewModel);
        RulesViewModel = new RulesViewModel(this, logger);
        GameData = new GameData(ScoreboardViewModel, GameViewModel);
        MainMenuViewModel = new MainMenuViewModel(this, logger, GameData);
        AddPlayerViewModel = new AddPlayerViewModel(this, logger, GameLogic);
        MultiplayerGamePageViewModel = new MPGamePageViewModel(this, logger, MultiplayerRoomsViewModel);
        LobbyViewModel = new LobbyViewModel(this, logger, MultiplayerRoomsViewModel);
        GiveNameViewModel = new GiveNameViewModel(this, logger, MultiplayerRoomsViewModel);
        CreateRoomViewModel = new CreateRoomViewModel(this, MultiplayerRoomsViewModel);
        PasswordViewModel = new PasswordViewModel(this, logger, MultiplayerRoomsViewModel);

        MainMenuVisible = true;
    }
    

    public void GoToMainMenu()
    {
        MainMenuVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToAddPlayer()
    {
        AddPlayerVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToSettings()
    {
        SettingsVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToGame()
    {
        GameVisible = true;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToMultiplayerGame()
    {
        MultiplayerGamePageVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        OnPropertyChanged(nameof(MultiplayerRoomsViewModel));
    }

    public void GoToScoreboard()
    {
        ScoreboardVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        GiveNameVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        AddPlayerVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
        GameData.Load();
    }

    public void GoToRules()
    {
        RulesVisible = true;
        GameVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        MainMenuVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToWinner()
    {
        WinnerVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        MainMenuVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public void GoToMultiplayerRooms()
    {
        MultiplayerRoomsVisible = true;
        GameVisible = false;
        RulesVisible = false;
        LobbyVisible = false;
        WinnerVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        MainMenuVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerGamePageVisible = false;
        MultiplayerRoomsViewModel.DisableAll = true;
    }

    public void GoToLobby()
    {
        LobbyVisible = true;
        GameVisible = false;
        RulesVisible = false;
        WinnerVisible = false;
        SettingsVisible = false;
        PasswordVisible = false;
        MainMenuVisible = false;
        GiveNameVisible = false;
        AddPlayerVisible = false;
        ScoreboardVisible = false;
        CreateRoomVisible = false;
        MultiplayerRoomsVisible = false;
        MultiplayerGamePageVisible = false;
    }

    public bool MainMenuVisible
    {
        get => mainMenuVisible;
        set
        {
            if (value == mainMenuVisible)
            {
                return;
            }

            mainMenuVisible = value;
            OnPropertyChanged();
        }
    }

    public bool WinnerVisible
    {
        get => winnerVisible;
        set
        {
            if (value == winnerVisible)
            {
                return;
            }

            winnerVisible = value;
            OnPropertyChanged();
        }
    }

    public bool RulesVisible
    {
        get => rulesVisible;
        set
        {
            if (value == rulesVisible)
            {
                return;
            }

            rulesVisible = value;
            OnPropertyChanged();
        }
    }


    public bool ScoreboardVisible
    {
        get => scoreboardVisible;
        set
        {
            if (value == scoreboardVisible)
            {
                return;
            }

            scoreboardVisible = value;
            OnPropertyChanged();
        }
    }

    public bool MultiplayerRoomsVisible
    {
        get => multiplayerRoomsVisible;
        set
        {
            if (value == multiplayerRoomsVisible)
            {
                return;
            }

            multiplayerRoomsVisible = value;
            OnPropertyChanged();
        }
    }

    public bool MultiplayerGamePageVisible
    {
        get => multiplayerGamePageVisible;
        set
        {
            if (value == multiplayerGamePageVisible)
            {
                return;
            }

            multiplayerGamePageVisible = value;
            OnPropertyChanged();
        }
    }

    public bool GameVisible
    {
        get => gameVisible;
        set
        {
            if (value == gameVisible)
            {
                return;
            }

            gameVisible = value;
            OnPropertyChanged();
        }
    }

    public bool SettingsVisible
    {
        get => settingsVisible;
        set
        {
            if (value == settingsVisible)
            {
                return;
            }

            settingsVisible = value;
            OnPropertyChanged();
        }
    }

    public bool AddPlayerVisible
    {
        get => addPlayerVisible;
        set
        {
            if (value == addPlayerVisible)
            {
                return;
            }

            addPlayerVisible = value;
            OnPropertyChanged();
        }
    }

    public bool CreateRoomVisible
    {
        get => createRoomVisible;
        set
        {
            if (value == createRoomVisible)
            {
                return;
            }

            createRoomVisible = value;
            OnPropertyChanged();
        }
    }

    public bool PasswordVisible
    {
        get => passwordVisible;
        set
        {
            if (value == passwordVisible)
            {
                return;
            }

            passwordVisible = value;
            OnPropertyChanged();
        }
    }

    public bool GiveNameVisible
    {
        get => giveNameVisible;
        set
        {
            if (value == giveNameVisible)
            {
                return;
            }

            giveNameVisible = value;
            OnPropertyChanged();
        }
    }

    public bool LobbyVisible
    {
        get => lobbyVisible;
        set
        {
            if (value == lobbyVisible)
            {
                return;
            }

            lobbyVisible = value;
            OnPropertyChanged();
        }
    }
}