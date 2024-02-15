using System.Collections.Generic;

namespace WpfApp2.Setting;

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