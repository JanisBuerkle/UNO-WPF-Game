using System.Collections.Generic;

namespace UNO_Spielprojekt.Setting;

public class WindowModes : ViewModelBase
{
    public List<WindowMode> MyModes { get; }

    public WindowModes()
    {
        MyModes = new List<WindowMode>
        {
            WindowMode.FullScreen,
            WindowMode.Windowed
        };
    }
}