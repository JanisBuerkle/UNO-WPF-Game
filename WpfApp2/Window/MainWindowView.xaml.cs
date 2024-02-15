using System.Windows;
using System.Windows.Input;
using WpfApp2.MultiplayerRooms;

namespace WpfApp2.Window;

public partial class MainWindowView
{
    public static MainWindowView? Instance { get; private set; }

    public MainWindowView()
    {
        Closing += MainWindow_Closing;
        InitializeComponent();
        Instance = this;
    }
    
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        // Code wird ausgeführt, wenn die MainWindow geschlossen wird
        // MultiplayerRoomsViewModel.UpdateOnlinePlayer(false);
    }
    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}