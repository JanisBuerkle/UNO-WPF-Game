using System.Collections.Generic;

namespace UNO_Spielprojekt.Setting;

public class ThemeModes : ViewModelBase
{
    private string _background;

    public string Background
    {
        get => _background;
        set
        {
            _background = value;
            OnPropertyChanged();
        }
    }

    private string _foreground;

    public string Foreground
    {
        get => _foreground;
        set
        {
            _foreground = value;
            OnPropertyChanged();
        }
    }

    public List<ThemeMode> MyThemeModes { get; }

    public ThemeModes()
    {
        Background = "#1f1f1f";
        Foreground = "#ffffff";
        MyThemeModes = new List<ThemeMode>
        {
            ThemeMode.Dark,
            ThemeMode.Bright
        };
    }
}