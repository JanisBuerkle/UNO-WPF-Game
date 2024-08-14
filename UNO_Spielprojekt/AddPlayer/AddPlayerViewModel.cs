using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.AddPlayer;

public class AddPlayerViewModel
{
    private readonly MainViewModel mainViewModel;

    private readonly ILogger logger;
    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand WeiterButtonCommand { get; }
    public ObservableCollection<NewPlayerViewModel> PlayerNames { get; }
    private GameLogic GameLogic { get; }

    public AddPlayerViewModel(MainViewModel mainViewModel, ILogger loggerr, GameLogic gameLogic)
    {
        logger = loggerr;
        this.mainViewModel = mainViewModel;
        GameLogic = gameLogic;
        PlayerNames = new ObservableCollection<NewPlayerViewModel>();

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        WeiterButtonCommand = new RelayCommand(WeiterButtonCommandMethod);
    }

    private void GoToMainMenuCommandMethod()
    {
        logger.Info("MainMenu wurde geöffnet.");
        mainViewModel.GoToMainMenu();
    }

    private void WeiterButtonCommandMethod()
    {
        foreach (var player in PlayerNames)
        {
            logger.Info($"Neuer Spieler: {player.Name} wurde hinzugefügt.");

            GameLogic.Players.Add(new Players { PlayerName = player.Name });
        }

        logger.Info("Rules Seite wurde geöffnet.");
        mainViewModel.GoToRules();
    }
}