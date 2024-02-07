using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using UNO_Spielprojekt.GamePage;
using UNO_Spielprojekt.Window;
using tt.Tools.Logging;
using UNO_Spielprojekt.Setting;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using UNO_Spielprojekt.AddPlayer;
using UNO_Spielprojekt.Service;

namespace UNO_Spielprojekt.AddPlayer;

public class AddPlayerViewModel : INotifyPropertyChanged
{
    private readonly MainViewModel _mainViewModel;
    private readonly ApiService _apiService;
    public RelayCommand GoToMainMenuCommand { get; }
    private GameLogic GameLogic { get; set; }
    public event PropertyChangedEventHandler? PropertyChanged;
    public RelayCommand WeiterButtonCommand { get; }

    private readonly ILogger _logger;

    public AddPlayerViewModel(MainViewModel mainViewModel, GameLogic gameLogic, ILogger logger, ThemeModes themeModes,
        ApiService apiService)
    {
        this._logger = logger;
        _apiService = apiService;
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

    private async void PostPlayers(string name)
    {
        var player = new Player
        {
            Id = 0,
            PlayerName = name,
            Uno = false
        };
        await _apiService.PostPlayerAsync(player);
    }

    public ObservableCollection<NewPlayerViewModel> PlayerNames { get; }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}