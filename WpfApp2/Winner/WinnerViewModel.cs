using CommunityToolkit.Mvvm.Input;
using WpfApp2.GamePage;
using WpfApp2.Setting;
using WpfApp2.Window;

namespace WpfApp2.Winner;

public class WinnerViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    public RelayCommand GoToMainMenuCommand { get; }

    private string _winnerName;

    public string WinnerName
    {
        get => _winnerName;
        set
        {
            if (_winnerName != value)
            {
                _winnerName = value;
                OnPropertyChanged();
            }
        }
    }

    private string _roundCounter;

    public string RoundCounter
    {
        get => _roundCounter;
        set
        {
            if (_roundCounter != value)
            {
                _roundCounter = value;
                OnPropertyChanged();
            }
        }
    }

    public WinnerViewModel(MainViewModel mainViewModel, ThemeModes themeModes)
    {
        _mainViewModel = mainViewModel;
        GoToMainMenuCommand = new RelayCommand(mainViewModel.GoToMainMenu);
    }
}