using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;

namespace UNO_Spielprojekt.Rules;

public class RulesViewModel
{
    public RelayCommand GoToGameCommand { get; }
    private readonly GameViewModel _gameViewModel;
    private readonly ILogger _logger;

    public RulesViewModel(MainViewModel mainViewModel, ILogger logger, ThemeModes themeModes)
    {
        _logger = logger;
        _gameViewModel = mainViewModel.GameViewModel;
        GoToGameCommand = new RelayCommand(GoToGameMethod);
    }

    private void GoToGameMethod()
    {
        _logger.Info("Spiel wird gestartet.");
        _gameViewModel.InitializeGame();
        _gameViewModel.SetCurrentHand();
        _gameViewModel.Game();
    }
}