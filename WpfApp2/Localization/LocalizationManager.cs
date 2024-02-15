using System;
using System.Globalization;
using System.Resources;
using WpfApp2.Resources;
using WpfApp2.Setting;

namespace WpfApp2.Localization;

public static class LocalizationManager
{
    private static ResourceManager _resourceManager;

    public static void SetLanguage(CultureInfo culture)
    {
        _resourceManager = new ResourceManager("WpfApp2.Resources.Resource", typeof(Resource).Assembly);

        var localizedResourceSet = _resourceManager.GetResourceSet(culture, true, true);

        if (localizedResourceSet != null)
            Console.WriteLine($@"Resources loaded for culture: {culture.Name}");
        else
            Console.WriteLine($@"No resources found for culture: {culture.Name}");
    }

    public static string? GetLocalizedString(string key)
    {
        if (_resourceManager == null)
            _resourceManager =
                new ResourceManager("WpfApp2.Resources.Resource", typeof(Resource).Assembly);

        if (SettingsView.language != null)
            return _resourceManager.GetString(key, SettingsView.language.LangCulture);
        return "DefaultLocalizedString";
    }
}