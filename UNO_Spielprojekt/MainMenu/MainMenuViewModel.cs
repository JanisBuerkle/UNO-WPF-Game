using System.Windows;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.Scoreboard;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MainMenu;

public class MainMenuViewModel : ViewModelBase
{
    private readonly MainViewModel mainViewModel;
    private readonly GameData gameData;
    private readonly ILogger logger;
    public RelayCommand GoToAddPlayerCommand { get; }
    public RelayCommand GoToMultiplayerRoomsCommand { get; }
    public RelayCommand GoToScoreboardCommand { get; }
    public RelayCommand GoToSettings { get; }
    public RelayCommand ExitApplicationCommand { get; }

    public MainMenuViewModel(MainViewModel mainViewModel, ILogger loggerr, GameData gameData)
    {
        this.gameData = gameData;
        this.mainViewModel = mainViewModel;
        logger = loggerr;
        GoToAddPlayerCommand = new RelayCommand(GoToAddPlayerCommandMethod);
        GoToMultiplayerRoomsCommand = new RelayCommand(GoToMultiplayerRoomsCommandMethod);
        GoToScoreboardCommand = new RelayCommand(GoToScoreboardCommandMethod);
        GoToSettings = new RelayCommand(GoToSettingsCommandMethod);
        ExitApplicationCommand = new RelayCommand(ExitApplication);
    }

    private void GoToAddPlayerCommandMethod()
    {
        logger.Info("AddPlayer Seite wurde geöffnet.");
        mainViewModel.GoToAddPlayer();
    }

    private void GoToMultiplayerRoomsCommandMethod()
    {
        logger.Info("MultiplayerRooms Seite wurde geöffnet.");
        mainViewModel.GoToMultiplayerRooms();
        mainViewModel.MultiplayerRoomsViewModel.GetRooms();
    }

    private void GoToScoreboardCommandMethod()
    {
        logger.Info("Scoreboard Seite wurde geöffnet.");
        gameData.Load();
        mainViewModel.GoToScoreboard();
    }

    private void GoToSettingsCommandMethod()
    {
        logger.Info("Settings Seite wurde geöffnet.");
        mainViewModel.GoToSettings();
    }

    private void ExitApplication()
    {
        logger.Info("Program wird mit dem 'Exit' Button beendet.");
        Application.Current.Shutdown();
    }
}