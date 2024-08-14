using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Scoreboard;

[Serializable]
public class ScoreboardViewModel : ViewModelBase
{
    private readonly MainViewModel mainViewModel;

    private List<ScoreboardPlayer> scoreboardPlayers = new();
    private ScoreboardViewModel scoreboardViewModel;

    public RelayCommand GoToMainMenuCommand { get; }
    public bool Start { get; set; }

    public ScoreboardViewModel(MainViewModel mainViewModel)
    {
        scoreboardViewModel = this;
        this.mainViewModel = mainViewModel;
        GoToMainMenuCommand = new RelayCommand(Test);
    }

    public void LoadGameData()
    {
        var sortedList = ScoreboardPlayers
            .OrderByDescending(ScoreboardPlayer => ScoreboardPlayer.PlayerScoreboardScore).ToList();
        ScoreboardPlayers.Clear();
        foreach (var player in sortedList)
        {
            ScoreboardPlayers.Add(player);
        }
    }

    public void Test()
    {
        mainViewModel.GoToMainMenu();
    }

    public List<ScoreboardPlayer> ScoreboardPlayers
    {
        get => scoreboardPlayers;
        set
        {
            if (scoreboardPlayers != value)
            {
                scoreboardPlayers = value;
                OnPropertyChanged();
            }
        }
    }
}