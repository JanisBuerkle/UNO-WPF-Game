using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;

namespace UNO_Spielprojekt.AddPlayer;

public class AddPlayerViewModel
{
    private GameLogic GameLogic { get; set; }

    private readonly MainViewModel _mainViewModel;
    public RelayCommand GoToMainMenuCommand { get; }
    public RelayCommand WeiterButtonCommand { get; }
    public ObservableCollection<NewPlayerViewModel> PlayerNames { get; }

    private readonly ILogger _logger;

    public AddPlayerViewModel(MainViewModel mainViewModel, ILogger logger, GameLogic gameLogic)
    {
        _logger = logger;
        _mainViewModel = mainViewModel;
        GameLogic = gameLogic;
        PlayerNames = new ObservableCollection<NewPlayerViewModel>();

        GoToMainMenuCommand = new RelayCommand(GoToMainMenuCommandMethod);
        WeiterButtonCommand = new RelayCommand(WeiterButtonCommandMethod);
    }

    private void GoToMainMenuCommandMethod()
    {
        _logger.Info("MainMenu wurde geöffnet.");
        _mainViewModel.GoToMainMenu();
    }

    private void WeiterButtonCommandMethod()
    {
        foreach (var player in PlayerNames)
        {
            _logger.Info($"Neuer Spieler: {player.Name} wurde hinzugefügt.");

            GameLogic.Players.Add(new Players { PlayerName = player.Name });
        }

        _logger.Info("Rules Seite wurde geöffnet.");
        _mainViewModel.GoToRules();
    }
}