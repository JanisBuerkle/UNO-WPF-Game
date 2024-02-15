using System.IO;
using System.Net;
using WpfApp2.GamePage;

namespace WpfApp2.Scoreboard;
public class GameData : ViewModelBase
{
    private readonly GameViewModel _gameViewModel;
    private readonly ScoreboardViewModel _scoreboardViewModel;
    public GameData(ScoreboardViewModel scoreboardViewModel, GameViewModel gameViewModel)
    {
        _gameViewModel = gameViewModel;
        _scoreboardViewModel = scoreboardViewModel;
        Load();
    }

    public void Load()
    {
        if (File.Exists("GameData.xml"))
        {
            _scoreboardViewModel.ScoreboardPlayers = _gameViewModel.LoadPlayersFromXml("GameData.xml");
            _scoreboardViewModel.LoadGameData();
        }
    }
}