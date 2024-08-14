using System.Collections.Generic;
using CommunityToolkit.Mvvm.Input;
using tt.Tools.Logging;
using UNO_Spielprojekt.Window;

namespace UNO_Spielprojekt.Setting;

public class SettingsViewModel : ViewModelBase
{
    private readonly ILogger logger;
    public List<Language> MyLangs { get; }
    public List<WindowMode> MyWindowModes { get; }
    public RelayCommand GoToMainMenuCommand { get; }

    public SettingsViewModel(MainViewModel mainViewModel, ILogger loggerr)
    {
        logger = loggerr;
        GoToMainMenuCommand = new RelayCommand(mainViewModel.GoToMainMenu);
        MyLangs = new List<Language>
        {
            new()
            {
                CultureName = "en-US", Flag = "pack://application:,,,/Assets/Languages/english.png",
                LangName = "English"
            },
            new()
            {
                CultureName = "de-DE", Flag = "pack://application:,,,/Assets/Languages/germany.png",
                LangName = "Deutsch"
            }
        };


        MyWindowModes = new List<WindowMode>
        {
            WindowMode.FullScreen,
            WindowMode.Windowed
        };

    }
}