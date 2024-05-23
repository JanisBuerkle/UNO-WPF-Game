using UNO_Spielprojekt.Scoreboard;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;
using System.Windows;

namespace UNO_Spielprojekt.MainMenu;

public class MainMenuViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    private readonly GameData _gameData;
    private readonly ILogger _logger;
    public RelayCommand GoToAddPlayerCommand { get; }
    public RelayCommand GoToMultiplayerRoomsCommand { get; }
    public RelayCommand GoToScoreboardCommand { get; }
    public RelayCommand GoToSettings { get; }
    public RelayCommand ExitApplicationCommand { get; }

    public MainMenuViewModel(MainViewModel mainViewModel, ILogger logger, GameData gameData)
    {
        _gameData = gameData;
        _mainViewModel = mainViewModel;
        _logger = logger;
        GoToAddPlayerCommand = new RelayCommand(GoToAddPlayerCommandMethod);
        GoToMultiplayerRoomsCommand = new RelayCommand(GoToMultiplayerRoomsCommandMethod);
        GoToScoreboardCommand = new RelayCommand(GoToScoreboardCommandMethod);
        GoToSettings = new RelayCommand(GoToSettingsCommandMethod);
        ExitApplicationCommand = new RelayCommand(ExitApplication);
    }

    private void GoToAddPlayerCommandMethod()
    {
        _logger.Info("AddPlayer Seite wurde geöffnet.");
        _mainViewModel.GoToAddPlayer();
    }

    private void GoToMultiplayerRoomsCommandMethod()
    {
        _logger.Info("MultiplayerRooms Seite wurde geöffnet.");
        _mainViewModel.GoToMultiplayerRooms();
        _mainViewModel.MultiplayerRoomsViewModel.GetRooms();
    }

    private void GoToScoreboardCommandMethod()
    {
        _logger.Info("Scoreboard Seite wurde geöffnet.");
        _gameData.Load();
        _mainViewModel.GoToScoreboard();
    }

    private void GoToSettingsCommandMethod()
    {
        _logger.Info("Settings Seite wurde geöffnet.");
        _mainViewModel.GoToSettings();
    }

    private void ExitApplication()
    {
        _logger.Info("Program wird mit dem 'Exit' Button beendet.");
        Application.Current.Shutdown();
    }
}