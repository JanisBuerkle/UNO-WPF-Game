using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Rules;

public class RulesViewModel
{
    private readonly GameViewModel gameViewModel;
    private readonly ILogger logger;
    public RelayCommand GoToGameCommand { get; }

    public RulesViewModel(MainViewModel mainViewModel, ILogger loggerr)
    {
        logger = loggerr;
        gameViewModel = mainViewModel.GameViewModel;
        GoToGameCommand = new RelayCommand(GoToGameMethod);
    }

    private void GoToGameMethod()
    {
        logger.Info("Spiel wird gestartet.");
        gameViewModel.InitializeGame();
        gameViewModel.SetCurrentHand();
        gameViewModel.Game();
    }
}