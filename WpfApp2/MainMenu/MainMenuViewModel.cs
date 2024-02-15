using System.Windows;
using CommunityToolkit.Mvvm.Input;
using WpfApp2.Window;
using tt.Tools.Logging;
using WpfApp2.Scoreboard;
using WpfApp2.Setting;

namespace WpfApp2.MainMenu;

public class MainMenuViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    private readonly GameData _gameData;
    private readonly ILogger logger;
    public RelayCommand GoToAddPlayerCommand { get; }
    public RelayCommand GoToMultiplayerRoomsCommand { get; }
    public RelayCommand GoToScoreboardCommand { get; }
    public RelayCommand GoToSettings { get; }
    public RelayCommand ExitApplicationCommand { get; }

    public MainMenuViewModel(MainViewModel mainViewModel, ILogger logger, GameData gameData, ThemeModes themeModes)
    {
        _gameData = gameData;
        _mainViewModel = mainViewModel;
        this.logger = logger;
        GoToAddPlayerCommand = new RelayCommand(GoToAddPlayerCommandMethod);
        GoToMultiplayerRoomsCommand = new RelayCommand(GoToMultiplayerRoomsCommandMethod);
        GoToScoreboardCommand = new RelayCommand(GoToScoreboardCommandMethod);
        GoToSettings = new RelayCommand(GoToSettingsCommandMethod);
        ExitApplicationCommand = new RelayCommand(ExitApplication);
    }

    private void GoToAddPlayerCommandMethod()
    {
        logger.Info("AddPlayer Seite wurde geöffnet.");
        _mainViewModel.GoToAddPlayer();
    }

    private void GoToMultiplayerRoomsCommandMethod()
    {
        logger.Info("MultiplayerRooms Seite wurde geöffnet.");
        _mainViewModel.GoToMultiplayerRooms();
        _mainViewModel.MultiplayerRoomsViewModel.GetRoom();
    }

    private void GoToScoreboardCommandMethod()
    {
        logger.Info("Scoreboard Seite wurde geöffnet.");
        _gameData.Load();
        _mainViewModel.GoToScoreboard();
    }

    private void GoToSettingsCommandMethod()
    {
        logger.Info("Settings Seite wurde geöffnet.");
        _mainViewModel.GoToSettings();
    }

    private void ExitApplication()
    {
        logger.Info("Program wird mit dem 'Exit' Button beendet.");
        Application.Current.Shutdown();
    }
}