﻿using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using UNO_Spielprojekt.Setting;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;
using System.Linq;
using System;

namespace UNO_Spielprojekt.Scoreboard;

[Serializable]
public class ScoreboardViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;

    private List<ScoreboardPlayer> _scoreboardPlayers = new();

    public List<ScoreboardPlayer> ScoreboardPlayers
    {
        get => _scoreboardPlayers;
        set
        {
            if (_scoreboardPlayers != value)
            {
                _scoreboardPlayers = value;
                OnPropertyChanged();
            }
        }
    }

    public RelayCommand GoToMainMenuCommand { get; }
    private ScoreboardViewModel _scoreboardViewModel;
    private readonly ILogger _logger;
    public bool Start { get; set; }

    public ScoreboardViewModel(MainViewModel mainViewModel, ILogger logger, ThemeModes themeModes)
    {
        _logger = logger;
        _scoreboardViewModel = this;
        _mainViewModel = mainViewModel;
        GoToMainMenuCommand = new RelayCommand(Test);
    }

    public void LoadGameData()
    {
        List<ScoreboardPlayer> sortedList = ScoreboardPlayers
            .OrderByDescending(ScoreboardPlayer => ScoreboardPlayer.PlayerScoreboardScore).ToList();
        ScoreboardPlayers.Clear();
        foreach (var player in sortedList)
        {
            ScoreboardPlayers.Add(player);
        }
    }

    public void Test()
    {
        _mainViewModel.GoToMainMenu();
    }
}