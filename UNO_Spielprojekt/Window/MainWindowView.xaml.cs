using System.Windows.Input;

namespace UNO_Spielprojekt.Window;

public partial class MainWindowView
{
    public static MainWindowView? Instance { get; private set; }

    public MainWindowView()
    {
        InitializeComponent();
        Instance = this;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
}