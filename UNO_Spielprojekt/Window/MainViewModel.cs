using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Logging;
using UNO_Spielprojekt.MainMenu;
using UNO_Spielprojekt.MultiplayerCreateRoom;
using UNO_Spielprojekt.MultiplayerGiveName;
using UNO_Spielprojekt.MultiplayerLobby;
using UNO_Spielprojekt.MultiplayerPassword;
using UNO_Spielprojekt.MultiplayerRooms;
using UNO_Spielprojekt.Rules;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Service;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Winner;

namespace UNO_Spielprojekt.Window;

public class MainViewModel : ViewModelBase
{
    public ThemeModes ThemeModes { get; }
    public CreateRoomViewModel CreateRoomViewModel { get; set; }
    public PasswordViewModel PasswordViewModel { get; set; }
    public GiveNameViewModel GiveNameViewModel { get; set; }
    public GameData GameData { get; set; }
    private ApiService ApiService { get; set; }
    public WinnerViewModel WinnerViewModel { get; }
    public MultiplayerRoomsViewModel MultiplayerRoomsViewModel { get; }
    public LobbyViewModel LobbyViewModel { get; }
    public GameViewModel GameViewModel { get; set; }
    public RulesViewModel RulesViewModel { get; set; }
    public SettingsViewModel SettingsViewModel { get; }
    public ScoreboardViewModel ScoreboardViewModel { get; }
    public MainMenuViewModel MainMenuViewModel { get; set; }
    public AddPlayerViewModel AddPlayerViewModel { get; set; }


    public MainViewModel()
    {
        var loggerFactory = new SerilogLoggerFactory();
        var logger = loggerFactory.CreateLogger("Uno-Spielprojekt");

        ApiService = new ApiService();
        ThemeModes = new ThemeModes();
        SettingsViewModel = new SettingsViewModel(this, logger, ThemeModes);
        PlayViewModel = new PlayViewModel();
        WinnerViewModel = new WinnerViewModel(this, ThemeModes);
        GameLogic = new GameLogic(PlayViewModel, logger, ApiService);
        ScoreboardViewModel = new ScoreboardViewModel(this, logger, ThemeModes);
        GameViewModel = new GameViewModel(this, logger, PlayViewModel, GameLogic, WinnerViewModel, ScoreboardViewModel,
            ThemeModes);
        RulesViewModel = new RulesViewModel(this, logger, ThemeModes);
        GameData = new GameData(ScoreboardViewModel, GameViewModel);
        MainMenuViewModel = new MainMenuViewModel(this, logger, GameData, ThemeModes);
        AddPlayerViewModel = new AddPlayerViewModel(this, logger, GameLogic, ThemeModes, ApiService);

        MultiplayerRoomsViewModel = new MultiplayerRoomsViewModel(this, logger, ApiService);
        LobbyViewModel = new LobbyViewModel(this, logger, MultiplayerRoomsViewModel);
        GiveNameViewModel = new GiveNameViewModel(this, logger, MultiplayerRoomsViewModel);
        CreateRoomViewModel = new CreateRoomViewModel(this, logger, MultiplayerRoomsViewModel, ApiService);
        PasswordViewModel = new PasswordViewModel(this, logger, MultiplayerRoomsViewModel);
        MainMenuVisible = true;
    }

    private bool _gameVisible;
    private bool _rulesVisible;
    private bool _winnerVisible;
    private bool _settingsVisible;
    private bool _mainMenuVisible;
    private bool _addPlayerVisible;
    private bool _scoreboardVisible;

    private bool _multiplayerRoomsVisible;
    private bool _createRoomVisible;
    private bool _passwordVisible;
    private bool _giveNameVisible;
    private bool _lobbyVisible;
    private readonly GameLogic GameLogic;
    private readonly PlayViewModel PlayViewModel;

    public bool MainMenuVisible
    {
        get => _mainMenuVisible;
        set
        {
            if (value == _mainMenuVisible) return;
            _mainMenuVisible = value;
            OnPropertyChanged();
        }
    }

    public bool WinnerVisible
    {
        get => _winnerVisible;
        set
        {
            if (value == _winnerVisible) return;
            _winnerVisible = value;
            OnPropertyChanged();
        }
    }

    public bool RulesVisible
    {
        get => _rulesVisible;
        set
        {
            if (value == _rulesVisible) return;
            _rulesVisible = value;
            OnPropertyChanged();
        }
    }


    public bool ScoreboardVisible
    {
        get => _scoreboardVisible;
        set
        {
            if (value == _scoreboardVisible) return;
            _scoreboardVisible = value;
            OnPropertyChanged();
        }
    }

    public bool MultiplayerRoomsVisible
    {
        get => _multiplayerRoomsVisible;
        set
        {
            if (value == _multiplayerRoomsVisible) return;
            _multiplayerRoomsVisible = value;
            OnPropertyChanged();
        }
    }

    public bool GameVisible
    {
        get => _gameVisible;
        set
        {
            if (value == _gameVisible) return;
            _gameVisible = value;
            OnPropertyChanged();
        }
    }

    public bool SettingsVisible
    {
        get => _settingsVisible;
        set
        {
            if (value == _settingsVisible) return;
            _settingsVisible = value;
            OnPropertyChanged();
        }
    }

    public bool AddPlayerVisible
    {
        get => _addPlayerVisible;
        set
        {
            if (value == _addPlayerVisible) return;
            _addPlayerVisible = value;
            OnPropertyChanged();
        }
    }

    public bool CreateRoomVisible
    {
        get => _createRoomVisible;
        set
        {
            if (value == _createRoomVisible) return;
            _createRoomVisible = value;
            OnPropertyChanged();
        }
    }

    public bool PasswordVisible
    {
        get => _passwordVisible;
        set
        {
            if (value == _passwordVisible) return;
            _passwordVisible = value;
            OnPropertyChanged();
        }
    }

    public bool GiveNameVisible
    {
        get => _giveNameVisible;
        set
        {
            if (value == _giveNameVisible) return;
            _giveNameVisible = value;
            OnPropertyChanged();
        }
    }

    public bool LobbyVisible
    {
        get => _lobbyVisible;
        set
        {
            if (value == _lobbyVisible) return;
            _lobbyVisible = value;
            OnPropertyChanged();
        }
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
    }
}