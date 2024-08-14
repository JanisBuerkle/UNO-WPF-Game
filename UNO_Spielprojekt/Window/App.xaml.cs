using System.Globalization;
using System.Threading;
using System.Windows;

namespace UNO_Spielprojekt.Window;

public partial class App : Application
{
    public App()
    {
        SetLanguage();
    }

    private static void SetLanguage()
    {
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
    }
}