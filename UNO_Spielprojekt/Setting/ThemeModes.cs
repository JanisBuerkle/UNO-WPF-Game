using System.Collections.Generic;
using System.Windows.Media;

namespace UNO_Spielprojekt.Setting;

public class ThemeModes : ViewModelBase
{
    private Brush _background;
    public Brush Background
    {
        get => _background;
        set
        {
            _background = value;
            OnPropertyChanged(nameof(Background));
        }
    }
    
    public List<ThemeMode> MyThemeModes { get; }

    public ThemeModes()
    {
        Background = Brushes.Black;
        MyThemeModes = new List<ThemeMode>
        {
            ThemeMode.Dark,
            ThemeMode.Bright
        };
    }
}