using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using WpfApp2.GamePage;
using WpfApp2.Setting;
using WpfApp2.Window;

namespace WpfApp2.Rules;

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