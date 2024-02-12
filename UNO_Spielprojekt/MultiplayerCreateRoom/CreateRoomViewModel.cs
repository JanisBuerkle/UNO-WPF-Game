using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.MultiplayerCreateRoom;

public class CreateRoomViewModel : ViewModelBase
{
    private bool _isChecked;
    private readonly ILogger _logger;
    private MainViewModel MainViewModel { get; set; }
    public RelayCommand CloseCreateRoomCommand { get; }
    public RelayCommand GiveNameCommand { get; }

    public bool IsChecked
    {
        get { return _isChecked; }
        set
        {
            _isChecked = value;
            OnPropertyChanged("IsChecked");
        }
    }

    private bool _isEnabled;

    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            if (_isEnabled != value)
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
    }


    public CreateRoomViewModel(MainViewModel mainViewModel, ILogger logger)
    {
        _logger = logger;
        MainViewModel = mainViewModel;
        GiveNameCommand = new RelayCommand(GiveNameCommandMethod);
        CloseCreateRoomCommand = new RelayCommand(CloseCreateRommCommandMethod);
    }

    private void GiveNameCommandMethod()
    {
        MainViewModel.CreateRoomVisible = false;
        MainViewModel.GiveNameVisible = true;
    }

    private void CloseCreateRommCommandMethod()
    {
        MainViewModel.CreateRoomVisible = false;
        MainViewModel.MultiplayerRoomsViewModel.DisableAll = true;
    }
}