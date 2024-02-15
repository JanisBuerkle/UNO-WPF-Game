using System.Windows;

namespace WpfApp2.Window
{
    public partial class App : Application
    {
        public App()
        {
            SetLanguage();
        }

        private static void SetLanguage()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
        }
    }
}