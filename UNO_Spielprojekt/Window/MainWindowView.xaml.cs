using System.Windows;
using System.Windows.Input;
using UNO_Spielprojekt.Setting;

namespace UNO_Spielprojekt.Window;

public partial class MainWindowView
{

    public static readonly DependencyProperty ThemeModesProperty = DependencyProperty.Register(
        nameof(ThemeModes), typeof(ThemeModes), typeof(MainWindowView), new PropertyMetadata(default(ThemeModes)));

    public ThemeModes ThemeModes
    {
        get => (ThemeModes)GetValue(ThemeModesProperty);
        set => SetValue(ThemeModesProperty, value);
    }
    
    
    public static MainWindowView Instance { get; private set; }

    public MainWindowView()
    {
        InitializeComponent();
        Instance = this;
    }

    private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}