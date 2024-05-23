using System.Collections.Generic;

namespace UNO_Spielprojekt.Setting;

public class WindowModes
{
    public List<WindowMode> MyModes { get; } = new()
    {
        WindowMode.FullScreen,
        WindowMode.Windowed
    };
}