using System.Windows;
using System.Windows.Input;
using UNO_Spielprojekt.Logging;
using UNO_Spielprojekt.Service;
using UNO.Contract;

namespace UNO_Spielprojekt.Window;

public partial class MainWindowView
{
    public static readonly DependencyProperty MainViewModelProperty = DependencyProperty.Register(
        nameof(MainViewModel), typeof(MainViewModel), typeof(MainWindowView), new PropertyMetadata(default(MainViewModel)));

    public MainViewModel MainViewModel
    {
        get => (MainViewModel)GetValue(MainViewModelProperty);
        set => SetValue(MainViewModelProperty, value);
    }

    public static MainWindowView? Instance { get; private set; }

    public MainWindowView()
    {
        InitializeComponent();
        Instance = this;

        var loggerFactory = new SerilogLoggerFactory();
        var roomClient = new RoomClient();
        MainViewModel = new MainViewModel(roomClient, loggerFactory);
        var service =  new HubService(MainViewModel, MainViewModel.MultiplayerRoomsViewModel);
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
}